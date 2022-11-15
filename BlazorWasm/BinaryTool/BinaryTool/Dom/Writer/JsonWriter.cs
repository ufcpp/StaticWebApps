using System.Text.Json;
using System.Text.Json.Serialization;

namespace BinaryTool.Dom.Writer;

public class JsonWriter : IWriter
{
    private readonly JsonSerializerOptions _options;

    public JsonWriter(JsonSerializerOptions options, string description)
    {
        _options = options;
        Description = $"JSON ({description})";
    }

    public static readonly JsonWriter Default = new(new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    }, "no-indent, escape");

    public static readonly JsonWriter Indent = new(new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    }, "indent, escape");

    public static readonly JsonWriter NoEscape = new(new()
    {
        Encoder = NoEscapingJavaScriptEncoder.Instance,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    }, "no-indent, no-escape");

    public static readonly JsonWriter IndentNoEscape = new(new()
    {
        WriteIndented = true,
        Encoder = NoEscapingJavaScriptEncoder.Instance,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    }, "indent, no-escape");

    public string Description { get; }

    public byte[] Write(IReadOnlyList<DomSpan> spans)
    {
        if (spans.Count == 0) return Array.Empty<byte>();
        if (spans[0].Value is Exception) return Array.Empty<byte>();
        return JsonSerializer.SerializeToUtf8Bytes(spans[0].Value, typeof(object), _options);
    }
}
