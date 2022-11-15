using System.Text;
using System.Text.Encodings.Web;

namespace BinaryTool.Dom.Writer;

internal class NoEscapingJavaScriptEncoder : JavaScriptEncoder
{
    public static JavaScriptEncoder Instance { get; } = new NoEscapingJavaScriptEncoder();

    public override int MaxOutputCharactersPerInputCharacter => 12;

    private static readonly HashSet<char> _escapingBmpChar = new() { '\\', '\"', '\r', '\n', '\t' };

    public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
    {
        for (int i = 0; i < textLength; i++)
        {
            if (WillEncode(text[i])) return i;
        }
        return -1;
    }

    public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
    {
        static void escape(char c, char* buffer)
        {
            buffer[0] = '\\';
            buffer[1] = 'u';
            ((ushort)c).TryFormat(new Span<char>(buffer + 2, 4), out _, "X4");
        }

        char? escapeChar = unicodeScalar switch
        {
            '\\' => '\\',
            '\r' => 'r',
            '\n' => 'n',
            '\t' => 't',
            '\"' => '\"',
            _ => null,
        };

        if (escapeChar is { } notNull)
        {
            if (bufferLength < 2)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            buffer[0] = '\\';
            buffer[1] = notNull;
            numberOfCharactersWritten = 2;
        }
        else if (char.IsControl((char)unicodeScalar))
        {
            escape((char)unicodeScalar, buffer);
            numberOfCharactersWritten = 6;
        }
        else if (unicodeScalar > 0xFFFF)
        {
            if (bufferLength < 6)
            {
                numberOfCharactersWritten = 0;
                return false;
            }

            var r = new Rune(unicodeScalar);
            Span<char> utf16 = stackalloc char[2];
            var len = r.EncodeToUtf16(utf16);

            buffer[0] = utf16[0];
            numberOfCharactersWritten = 1;

            if (len > 1)
            {
                buffer[1] = utf16[1];
                numberOfCharactersWritten = 2;
            }
        }
        else
        {
            buffer[0] = (char)unicodeScalar;
            numberOfCharactersWritten = 1;
        }

        return true;
    }

    public override bool WillEncode(int unicodeScalar)
    {
        return char.IsControl((char)unicodeScalar) || (unicodeScalar <= 0xFFFF && _escapingBmpChar.Contains((char)unicodeScalar));
    }
}
