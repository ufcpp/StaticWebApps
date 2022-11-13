using System.Text.RegularExpressions;

namespace BinaryTool.Binary.Parser;

internal partial class ParserHelper
{
    public static byte[] Parse(string input, bool hex = false)
    {
        var list = new List<byte>();
        var style = hex ? System.Globalization.NumberStyles.HexNumber : System.Globalization.NumberStyles.None;

        foreach (var m in Digits().EnumerateMatches(input))
        {
            var span = input.AsSpan(m.Index, m.Length);
            if (byte.TryParse(span, style, null, out var result))
                list.Add(result);
        }

        return list.ToArray();
    }

    [GeneratedRegex("""
        [\da-zA-Z]+
        """)]
    private static partial Regex Digits();
}
