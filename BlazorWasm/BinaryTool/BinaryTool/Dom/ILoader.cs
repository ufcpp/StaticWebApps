using System.Collections.Immutable;

namespace BinaryTool.Dom;

public interface ILoader : IDescriptiveItem
{
    (List<DomSpan> spans, bool isBinary) Parse(byte[] data);

    public static readonly ImmutableArray<ILoader> DefaultLoaders = ImmutableArray.Create(new ILoader[]
    {
        JsonLoader.Instance,
        MessagePackLoader.Instance,
    });

    public static readonly ImmutableArray<ILoader> DefaultLoadersWithAuto = ImmutableArray.CreateRange(DefaultLoaders.Prepend(AutoDetectLoader.Instance));
}
