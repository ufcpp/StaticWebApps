namespace BinaryTool.Binary.Parser;

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
            _ => Utf8Parser.Instance,
        };
    }
}
