using System.Buffers.Text;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryTool.Binary;

public interface IParser : IDescriptiveItem
{
    byte[] Parse(string input);

    public static readonly ImmutableArray<IParser> DefaultParsers = ImmutableArray.Create(new IParser[]
    {
        AutoDetectParser.Instance,
        HexParser.Instance,
        ConsecutiveHexParser.Instance,
        DecimalParser.Instance,
        Base64Parser.Instance,
        Utf8Parser.Instance,
    });
}

//todo: C# style?
// 123, 0x12, 0b11, 1_23, 0x1_2, 0b1111_1111, ...

public class AutoDetectParser : IParser
{
    public static readonly AutoDetectParser Instance = new();

    public string Description => "auto detect";

    public byte[] Parse(string input)
    {
        var p = Detect(input);
        return p?.Parse(input) ?? Array.Empty<byte>();
    }

    private static IParser? Detect(ReadOnlySpan<char> input)
    {
        var other = false;
        var number = false;
        var lower = false;
        var upper = false;
        var base64 = false;
        var separetor = false;

        int prevCat = 0;
        int count = 0;
        int min = int.MaxValue;
        int max = 0;
        bool initialZero = false;

        foreach (var c in input)
        {
            var cat = c switch
            {
                >= '0' and <= '9' => 1,
                >= 'a' and <= 'z' => 2,
                >= 'A' and <= 'Z' => 3,
                '+' or '/' or '-' or '_' or ':' or '=' => 4,
                ',' or ' ' or '\r' or '\n' or '\t' => 5,
                _ => 0,
            };

            switch (cat)
            {
                case 1: number = true; break;
                case 2: lower = true; break;
                case 3: upper = true; break;
                case 4: base64 = true; break;
                case 5: separetor = true; break;
                default: other = true; break;
            }

            if (cat == 1)
            {
                count++;

                if (prevCat != 1)
                {
                    if (c == '0') initialZero = true;
                }
            }
            else
            {
                count = 0;

                if (prevCat == 1)
                {
                    max = Math.Max(max, count);
                    min = Math.Min(min, count);
                }
            }

            prevCat = cat;
        }

        if (prevCat == 1)
        {
            max = Math.Max(max, count);
            min = Math.Min(min, count);
        }

        var alphabet = lower | upper;
        var mixedLetter = lower & upper;

        return (number, alphabet, mixedLetter, base64, separetor, other) switch
        {
            (true, true, false, false, false, false) => ConsecutiveHexParser.Instance,
            (true, _, false, false, false, false) =>
                input.Length >= 4 ? ConsecutiveHexParser.Instance :
                lower | upper ? HexParser.Instance :
                DecimalParser.Instance,
            (true, false, _, false, _, false) when !initialZero && (max > 2 || min != 2) => DecimalParser.Instance,
            (_, _, false, false, _, false) => HexParser.Instance,
            (_, _, _, _, false, false) => Base64Parser.Instance,
            _ => null,
        };
    }
}

public class DecimalParser : IParser
{
    public static readonly DecimalParser Instance = new();

    public string Description => "decimal numbers";
    public byte[] Parse(string input) => Parser.Parse(input, false);
}

public class HexParser : IParser
{
    public static readonly HexParser Instance = new();

    public string Description => "hexadecimal numbers";
    public byte[] Parse(string input) => Parser.Parse(input, true);
}

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

public class Base64Parser : IParser
{
    public static readonly Base64Parser Instance = new();

    public string Description => "base64";
    public byte[] Parse(string input)
    {
        try
        {
            return Convert.FromBase64String(input);
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }
}

public class Utf8Parser : IParser
{
    public static readonly Utf8Parser Instance = new();

    public string Description => "UTF-8";
    public byte[] Parse(string input) => Encoding.UTF8.GetBytes(input);
}

public partial class Parser
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
