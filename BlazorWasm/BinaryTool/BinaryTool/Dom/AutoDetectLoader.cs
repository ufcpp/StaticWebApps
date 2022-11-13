namespace BinaryTool.Dom;

public class AutoDetectLoader : ILoader
{
    public static readonly AutoDetectLoader Instance = new();

    public string Description => "auto detect";

    public (List<DomSpan> spans, bool isBinary) Parse(byte[] data)
    {
        var loaders = ILoader.DefaultLoaders;

        var maxCount = 0;
        (List<DomSpan> spans, bool isBinary) max = default;

        for (int i = 0; i < loaders.Length; i++)
        {
            var loader = loaders[i];
            var t = loader.Parse(data);
            var spans = t.spans;

            if (spans.Count != 0 && spans[^1].Kind != DomKind.Error) return t;

            if(spans.Count > maxCount)
            {
                maxCount = spans.Count;
                max = t;
            }
        }

        return max;
    }
}
