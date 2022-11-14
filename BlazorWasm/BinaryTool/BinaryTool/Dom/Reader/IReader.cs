using System.Collections.Immutable;

namespace BinaryTool.Dom.Reader;

public interface IReader : IDescriptiveItem
{
    (List<DomSpan> spans, bool isBinary) Read(byte[] data);

    public static readonly ImmutableArray<IReader> Readers = ImmutableArray.Create(new IReader[]
    {
        JsonReader.Instance,
        MessagePackReader.Instance,
    });

    public static readonly ImmutableArray<IReader> DefaultReaders = ImmutableArray.CreateRange(Readers.Prepend(AutoDetectReader.Instance));
}
