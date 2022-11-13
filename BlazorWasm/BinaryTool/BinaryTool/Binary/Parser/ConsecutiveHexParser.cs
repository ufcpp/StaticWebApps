namespace BinaryTool.Binary.Parser;

public class ConsecutiveHexParser : IParser
{
    public static readonly ConsecutiveHexParser Instance = new();

    public string Description => "consecutive hexadecimal numbers";
    public byte[] Parse(string input)
    {
        var span = input.AsSpan();
        var digLen = (span.Length + 1) / 2;
        var digits = new byte[digLen];

        for (int i = 0; i < digLen; i++)
        {
            var len = span.Length == 1 ? 1 : 2;
            var dig = span[..len];

            if (!byte.TryParse(dig, System.Globalization.NumberStyles.HexNumber, null, out var result))
                return Array.Empty<byte>();

            digits[i] = result;

            span = span[len..];
        }

        return digits;
    }
}
