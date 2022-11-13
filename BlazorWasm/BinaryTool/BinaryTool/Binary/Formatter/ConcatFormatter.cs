using System.Text;

namespace BinaryTool.Binary.Formatter;

public class ConcatFormatter : IFormatter
{
    public static readonly ConcatFormatter SpaceSeparatedDec = new("", " ", "", NumberFormat.Decimal, false);
    public static readonly ConcatFormatter SpaceSeparatedHex = new("", " ", "", NumberFormat.Hex, false);
    public static readonly ConcatFormatter ConsecutiveHex = new("", "", "", NumberFormat.Hex, false);
    public static readonly ConcatFormatter CsharpDec = new("new byte[] { ", ", ", " }", NumberFormat.Decimal, false);
    public static readonly ConcatFormatter CsharpDecMultiline = new("new byte[] {\n", ",\n", "}\n", NumberFormat.Decimal, true);
    public static readonly ConcatFormatter CsharpHex = new("new byte[] { ", ", ", " }", NumberFormat.Hex0x, false);
    public static readonly ConcatFormatter CsharpHexMultiline = new("new byte[] {\n", ",\n", "}\n", NumberFormat.Hex0x, true);
    public static readonly ConcatFormatter CollectionLiteralDec = new("[", ",", "]", NumberFormat.Decimal, false);
    public static readonly ConcatFormatter CollectionLiteralDecMultiline = new("[\n", ",\n", "]\n", NumberFormat.Decimal, true);
    public static readonly ConcatFormatter CollectionLiteralHex = new("[", ",", "]", NumberFormat.Hex0x, false);
    public static readonly ConcatFormatter CollectionLiteralHexMultiline = new("[\n", ",\n", "]\n", NumberFormat.Hex0x, true);

    public string Header { get; }
    public string Separator { get; }
    public string Footer { get; }
    public NumberFormat NumberFormat { get; }
    public bool TraillingSeparator { get; }

    public ConcatFormatter(string header, string separator, string footer, NumberFormat format, bool trallingSeparator)
    {
        Header = header;
        Separator = separator;
        Footer = footer;
        NumberFormat = format;
        TraillingSeparator = trallingSeparator;
    }

    private string? _desc;
    public string Description => _desc ??= Format(new byte[] { 1, 10, 100 });

    public string Format(ReadOnlySpan<byte> data)
    {
        var s = new StringBuilder();

        s.Append(Header);

        var part = false;
        foreach (var x in data)
        {
            if (!part) part = true;
            else s.Append(Separator);

            _ = NumberFormat switch
            {
                NumberFormat.Hex => s.Append($"{x:X2}"),
                NumberFormat.Hex0x => s.Append($"0x{x:X2}"),
                NumberFormat.Hex0X => s.Append($"0X{x:X2}"),
                _ => s.Append(x),
            };
        }

        if (TraillingSeparator) s.Append(Separator);

        s.Append(Footer);

        return s.ToString();
    }
}
