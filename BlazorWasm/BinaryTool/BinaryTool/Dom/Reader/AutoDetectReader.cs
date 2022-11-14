namespace BinaryTool.Dom.Reader;

public class AutoDetectReader : IReader
{
    public static readonly AutoDetectReader Instance = new();

    public string Description => "auto detect";

    public (List<DomSpan> spans, bool isBinary) Read(byte[] data)
    {
        var loaders = IReader.Readers;

        var maxCount = 0;
        (List<DomSpan> spans, bool isBinary) max = default;

        for (int i = 0; i < loaders.Length; i++)
        {
            var loader = loaders[i];
            var t = loader.Read(data);
            var spans = t.spans;

            if (spans.Count != 0 && spans[^1].Kind != DomKind.Error) return t;

            if (spans.Count > maxCount)
            {
                maxCount = spans.Count;
                max = t;
            }
        }

        return max;
    }
}
