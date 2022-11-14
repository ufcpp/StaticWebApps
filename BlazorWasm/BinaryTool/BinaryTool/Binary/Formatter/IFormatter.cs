using System.Collections.Immutable;

namespace BinaryTool.Binary.Formatter;

public interface IFormatter : IDescriptiveItem
{
    string Format(ReadOnlySpan<byte> data);

    public static readonly ImmutableArray<IFormatter> Defaults = ImmutableArray.Create(new IFormatter[]
    {
        Utf8Formatter.Instance,
        ConcatFormatter.SpaceSeparatedHex,
        ConcatFormatter.SpaceSeparatedDec,
        ConcatFormatter.ConsecutiveHex,
        ConcatFormatter.CsharpHex,
        ConcatFormatter.CsharpHexMultiline,
        ConcatFormatter.CsharpDec,
        ConcatFormatter.CsharpDecMultiline,
        ConcatFormatter.CollectionLiteralHex,
        ConcatFormatter.CollectionLiteralHexMultiline,
        ConcatFormatter.CollectionLiteralDec,
        ConcatFormatter.CollectionLiteralDecMultiline,
    });

    public static ImmutableArray<IFormatter> WithAutoDetect() => ImmutableArray.CreateRange(
        Defaults.Prepend(new AutoDetectFormatter()));
}
