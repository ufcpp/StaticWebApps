using System.Text.Json;

namespace BinaryTool.Dom;

public class JsonLoader : ILoader
{
    public static readonly JsonLoader Instance = new();

    public string Description => "JSON";

    public (List<DomSpan> spans, bool isBinary) Parse(byte[] data)
    {
        var reader = new Utf8JsonReader(data);
        var builder = new DomSpanBuilder();
        Parse(ref reader, builder);
        builder.PopAll((int)reader.BytesConsumed);
        return (builder.Results, false);
    }

    private static void Parse(ref Utf8JsonReader r, DomSpanBuilder builder)
    {
        while (true)
        {
            try
            {
                if (!r.Read()) break;
            }
            catch (JsonException ex)
            {
                builder.Exception(ex, (int)r.BytesConsumed..);
                return;
            }

            switch (r.TokenType)
            {
                case JsonTokenType.None:
                    break;
                case JsonTokenType.StartObject:
                    builder.PushMap((int)r.TokenStartIndex);
                    break;
                case JsonTokenType.StartArray:
                    builder.PushList((int)r.TokenStartIndex);
                    break;
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    builder.Pop((int)r.BytesConsumed);
                    break;
                case JsonTokenType.PropertyName:
                    builder.Key(r.GetString()!);
                    break;
                case JsonTokenType.Comment:
                    break;
                case JsonTokenType.String:
                    builder.Add(r.GetString(), r.Range());
                    break;
                case JsonTokenType.Number:
                    object? num = r.TryGetInt64(out var l) ? l : r.TryGetDouble(out var d) ? d : null;
                    builder.Add(num, r.Range());
                    break;
                case JsonTokenType.True:
                    builder.Add(true, r.Range());
                    break;
                case JsonTokenType.False:
                    builder.Add(false, r.Range());
                    break;
                case JsonTokenType.Null:
                    builder.Add(null, r.Range());
                    break;
            }
        }
    }
}

file static class Ex
{
    public static Range Range(this Utf8JsonReader r) => (int)r.TokenStartIndex..(int)r.BytesConsumed;
}
