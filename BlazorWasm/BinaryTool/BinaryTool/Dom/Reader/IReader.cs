using System.Collections.Immutable;

namespace BinaryTool.Dom.Reader;

public interface IReader : IDescriptiveItem
{
    (List<DomSpan> spans, bool isBinary) Read(byte[] data);

    public static readonly ImmutableArray<IReader> Defaults = ImmutableArray.Create(new IReader[]
    {
        JsonReader.Instance,
        MessagePackReader.Instance,
    });

    public static ImmutableArray<IReader> WithAutoDetect() => ImmutableArray.CreateRange(
        Defaults.Prepend(new AutoDetectReader()));
}
