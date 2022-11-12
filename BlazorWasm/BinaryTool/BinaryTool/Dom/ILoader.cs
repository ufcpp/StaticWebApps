using System.Collections.Immutable;

namespace BinaryTool.Dom;

public interface ILoader : IDescriptiveItem
{
    List<DomSpan> Parse(byte[] data);

    public static readonly ImmutableArray<ILoader> DefaultLoaders = ImmutableArray.Create(new ILoader[]
    {
        JsonLoader.Instance,
    });
}
