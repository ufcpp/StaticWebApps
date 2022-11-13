using System.Collections.Immutable;

namespace BinaryTool.Binary.Formatter;

public interface IFormatter : IDescriptiveItem
{
    string Format(ReadOnlySpan<byte> data);

    public static readonly ImmutableArray<IFormatter> DefaultFormatters = ImmutableArray.Create(new IFormatter[]
    {
        ConcatFormatter.SpaceSeparatedHex,
        ConcatFormatter.SpaceSeparatedDec,
        ConcatFormatter.ConsecutiveHex,
        Utf8Formatter.Instance,
        ConcatFormatter.CsharpHex,
        ConcatFormatter.CsharpHexMultiline,
        ConcatFormatter.CsharpDec,
        ConcatFormatter.CsharpDecMultiline,
        ConcatFormatter.CollectionLiteralHex,
        ConcatFormatter.CollectionLiteralHexMultiline,
        ConcatFormatter.CollectionLiteralDec,
        ConcatFormatter.CollectionLiteralDecMultiline,
    });
}
