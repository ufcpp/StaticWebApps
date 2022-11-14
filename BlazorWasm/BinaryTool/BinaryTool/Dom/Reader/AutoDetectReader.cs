namespace BinaryTool.Dom.Reader;

public class AutoDetectReader : IReader
{
    private const string _desc = "auto detect";

    public string Description { get; set; } = _desc;

    public (List<DomSpan> spans, bool isBinary) Read(byte[] data)
    {
        var readers = IReader.Defaults;

        IReader? maxItem = null;
        var maxCount = 0;
        (List<DomSpan> spans, bool isBinary) max = default;

        for (int i = 0; i < readers.Length; i++)
        {
            var reader = readers[i];
            var t = reader.Read(data);
            var spans = t.spans;

            if (spans.Count != 0 && spans[^1].Kind != DomKind.Error)
            {
                Description = $"{_desc} ({reader.Description})";
                return t;
            }

            if (spans.Count > maxCount)
            {
                maxCount = spans.Count;
                max = t;
                maxItem = reader;
            }
        }

        Description = $"{_desc} ({maxItem?.Description})";

        return max;
    }
}
