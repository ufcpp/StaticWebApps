using System.Collections.Immutable;
using System.Text;

namespace BinaryTool.Binary;

public interface IFormatter : IDescriptiveItem
{
    string Format(ReadOnlySpan<byte> data);

    public static readonly ImmutableArray<IFormatter> DefaultFormatters = ImmutableArray.Create(new IFormatter[]
    {
        Concat.SpaceSeparatedHex,
        Concat.SpaceSeparatedDec,
        Concat.ConsecutiveHex,
        Utf8.Instance,
        Concat.CsharpHex,
        Concat.CsharpHexMultiline,
        Concat.CsharpDec,
        Concat.CsharpDecMultiline,
        Concat.CollectionLiteralHex,
        Concat.CollectionLiteralHexMultiline,
        Concat.CollectionLiteralDec,
        Concat.CollectionLiteralDecMultiline,
    });
}

public class Utf8 : IFormatter
{
    public static readonly Utf8 Instance = new();

    public string Description => "as UTF-8";
    public string Format(ReadOnlySpan<byte> data) => Encoding.UTF8.GetString(data);
}

public enum NumberFormat
{
    Decimal,
    Hex,
    Hex0x,
    Hex0X,
}

public class Concat : IFormatter
{
    public static readonly Concat SpaceSeparatedDec = new("", " ", "", NumberFormat.Decimal, false);
    public static readonly Concat SpaceSeparatedHex = new("", " ", "", NumberFormat.Hex, false);
    public static readonly Concat ConsecutiveHex = new("", "", "", NumberFormat.Hex, false);
    public static readonly Concat CsharpDec = new("new byte[] { ", ", ", " }", NumberFormat.Decimal, false);
    public static readonly Concat CsharpDecMultiline = new("new byte[] {\n", ",\n", "}\n", NumberFormat.Decimal, true);
    public static readonly Concat CsharpHex = new("new byte[] { ", ", ", " }", NumberFormat.Hex0x, false);
    public static readonly Concat CsharpHexMultiline = new("new byte[] {\n", ",\n", "}\n", NumberFormat.Hex0x, true);
    public static readonly Concat CollectionLiteralDec = new("[", ",", "]", NumberFormat.Decimal, false);
    public static readonly Concat CollectionLiteralDecMultiline = new("[\n", ",\n", "]\n", NumberFormat.Decimal, true);
    public static readonly Concat CollectionLiteralHex = new("[", ",", "]", NumberFormat.Hex0x, false);
    public static readonly Concat CollectionLiteralHexMultiline = new("[\n", ",\n", "]\n", NumberFormat.Hex0x, true);

    public string Header { get; }
    public string Separator { get; }
    public string Footer { get; }
    public NumberFormat NumberFormat { get; }
    public bool TraillingSeparator { get; }

    public Concat(string header, string separator, string footer, NumberFormat format, bool trallingSeparator)
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
