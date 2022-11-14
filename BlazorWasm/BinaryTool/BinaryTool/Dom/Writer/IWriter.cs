using System.Collections.Immutable;

namespace BinaryTool.Dom.Writer;

public interface IWriter : IDescriptiveItem
{
    byte[] Write(IReadOnlyList<DomSpan> spans);

    public static readonly ImmutableArray<IWriter> DefaultWriters = ImmutableArray.Create(new IWriter[]
    {
        JsonWriter.Instance,
        MessagePackWriter.Instance,
    });
}
