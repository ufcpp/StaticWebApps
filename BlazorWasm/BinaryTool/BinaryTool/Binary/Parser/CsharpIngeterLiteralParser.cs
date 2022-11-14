namespace BinaryTool.Binary.Parser;

public class CsharpIngeterLiteralParser : IParser
{
    public static readonly CsharpIngeterLiteralParser Instance = new();

    public string Description => "C#-like";

    public byte[] Parse(string input)
    {
        int i = 0;

        static bool isWhitespace(char c)
            => char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.SpaceSeparator
                    || c is '\t' or '\u000B' or '\u000C';

        static bool isTerminator(char c)
            => c is ',' || isWhitespace(c);

        void skipWhitespace()
        {
            for (; i < input.Length && isWhitespace(input[i]); i++) ;
        }

        static int dec(char c) => c switch
        {
            >= '0' and <= '9' => c - '0',
            _ => -1,
        };

        static int hex(char c) => c switch
        {
            >= '0' and <= '9' => c - '0',
            >= 'a' and <= 'f' => c - 'a' + 10,
            >= 'A' and <= 'F' => c - 'A' + 10,
            _ => -1,
        };

        int parseHex()
        {
            var d = 0;

            for (; i < input.Length; i++)
            {
                var c = input[i];
                if (c is '_') continue;
                if (isTerminator(c)) break;
                var x = hex(c);
                if (x < 0) return -1;
                d = 16 * d + x;
            }

            return d;
        }

        int parseDec()
        {
            var d = 0;

            for (; i < input.Length; i++)
            {
                var c = input[i];
                if (c is '_') continue;
                if (isTerminator(c)) break;
                var x = dec(c);
                if (x < 0) return -1;
                d = 10 * d + x;
            }

            return d;
        }

        int parseBin()
        {
            var d = 0;

            for (; i < input.Length; i++)
            {
                var c = input[i];
                if (c is '_') continue;
                if (isTerminator(c)) break;
                var x = c switch { '0' => 0, '1' => 1, _ => -1 };
                if (x < 0) return -1;
                d = 2 * d + x;
            }

            return d;
        }

        int parse()
        {
            var c0 = input[i];

            if (c0 == '0')
            {
                ++i;
                if (i >= input.Length) return 0;

                var c1 = input[i++];

                if (c1 is 'x' or 'X') return parseHex();
                if (c1 is 'b' or 'B') return parseBin();
            }
            return parseDec();
        }

        var list = new List<byte>();
        while (true)
        {
            skipWhitespace();
            if (i >= input.Length) break;

            var d = parse();

            if (d < 0 || d >= 256) return Array.Empty<byte>();

            list.Add((byte)d);

            if (i >= input.Length) break;

            skipWhitespace();
            if (i >= input.Length) break;

            if (input[i] is ',') ++i;
        }

        return list.ToArray();
    }
}

//todo: C# style?
// 123, 0x12, 0b11, 1_23, 0x1_2, 0b1111_1111, ...
