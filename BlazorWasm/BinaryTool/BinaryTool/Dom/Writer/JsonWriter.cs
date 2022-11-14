using System.Text.Json;

namespace BinaryTool.Dom.Writer;

public class JsonWriter : IWriter
{
    public static readonly JsonWriter Instance = new();

    public string Description => "JSON";

    public byte[] Write(IReadOnlyList<DomSpan> spans)
    {
        if (spans.Count == 0) return Array.Empty<byte>();
        return JsonSerializer.SerializeToUtf8Bytes(spans[0].Value, typeof(object));
    }
}
