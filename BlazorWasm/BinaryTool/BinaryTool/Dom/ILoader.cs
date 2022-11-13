using System.Collections.Immutable;

namespace BinaryTool.Dom;

public interface ILoader : IDescriptiveItem
{
    (List<DomSpan> spans, bool isBinary) Parse(byte[] data);

    public static readonly ImmutableArray<ILoader> Loaders = ImmutableArray.Create(new ILoader[]
    {
        JsonLoader.Instance,
        MessagePackLoader.Instance,
    });

    public static readonly ImmutableArray<ILoader> DefaultLoaders = ImmutableArray.CreateRange(Loaders.Prepend(AutoDetectLoader.Instance));
}
