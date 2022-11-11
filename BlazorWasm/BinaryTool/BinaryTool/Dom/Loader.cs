using System.Text.Json;

using List = System.Collections.Generic.List<object?>;
using Map = System.Collections.Generic.Dictionary<object, object?>;

namespace BinaryTool.Dom;

public enum DomKind
{
    Unknown,
    Object,
    Array,
    Value,

    Error = -1,
}

public interface ILoader
{
    string Description { get; }
    List<DomSpan> Parse(byte[] data);
}

public record DomSpan(DomKind Kind, Range Range, object? Value, object? Key, int Length);

public class JsonLoader : ILoader
{
    public static readonly JsonLoader Instance = new JsonLoader();

    public string Description => "JSON";

    public List<DomSpan> Parse(byte[] data)
    {
        var reader = new Utf8JsonReader(data);
        var result = new List<DomSpan>();
        Parse(ref reader, result, null);
        return result;
    }

    private void Parse(ref Utf8JsonReader r, List<DomSpan> results, object? parent)
    {
        string? name = null;

        void add(object? value, Range range)
        {
            if (parent is Map m)
            {
                results.Add(new(DomKind.Value, range, value, name, 0));
                m.Add(name!, value);
            }
            else if (parent is List l)
            {
                results.Add(new(DomKind.Value, range, value, l.Count, 0));
                l.Add(value);
            }
            else
            {
                results.Add(new(DomKind.Value, range, value, null, 0));
            }
        }

        void setMap(int index, Map value, Range range) => set(index, DomKind.Object, value.Count, value, range);
        void setList(int index, List value, Range range) => set(index, DomKind.Array, value.Count, value, range);

        void set(int index, DomKind kind, int length, object? value, Range range)
        {
            if (parent is Map m)
            {
                results[index] = new(kind, range, value, name, length);
                m.Add(name!, value);
            }
            else if (parent is List l)
            {
                results[index] = new(kind, range, value, l.Count, length);
                l.Add(value);
            }
            else
            {
                results[index] = new(kind, range, value, null, length);
            }
        }

        while (true)
        {
            JsonException? ex = null;

            try
            {
                if (!r.Read()) break;
            }
            catch (JsonException e) { ex = e; }
            if (ex != null)
            {
                results.Add(new(DomKind.Error, ((int)r.BytesConsumed).., ex, null, 0));
                return;
            }

            switch (r.TokenType)
            {
                case JsonTokenType.None:
                    break;
                case JsonTokenType.StartObject:
                    {
                        var obj = new Map();
                        var index = results.Count;
                        results.Add(default!);
                        var start = (int)r.TokenStartIndex;
                        Parse(ref r, results, obj);
                        var end = (int)r.BytesConsumed;
                        setMap(index, obj, start..end);
                    }
                    break;
                case JsonTokenType.StartArray:
                    {
                        var obj = new List();
                        var index = results.Count;
                        results.Add(default!);
                        var start = (int)r.TokenStartIndex;
                        Parse(ref r, results, obj);
                        var end = (int)r.BytesConsumed;
                        setList(index, obj, start..end);
                    }
                    break;
                case JsonTokenType.EndObject:
                case JsonTokenType.EndArray:
                    return;
                case JsonTokenType.PropertyName:
                    name = r.GetString();
                    break;
                case JsonTokenType.Comment:
                    break;
                case JsonTokenType.String:
                    add(r.GetString(), r.Range());
                    break;
                case JsonTokenType.Number:
                    object? num = r.TryGetInt64(out var l) ? l : r.TryGetDouble(out var d) ? d : null;
                    add(num, r.Range());
                    break;
                case JsonTokenType.True:
                    add(true, r.Range());
                    break;
                case JsonTokenType.False:
                    add(false, r.Range());
                    break;
                case JsonTokenType.Null:
                    add(null, r.Range());
                    break;
            }
        }
    }
}

file static class Ex
{
    public static Range Range(this Utf8JsonReader r) => (int)r.TokenStartIndex..(int)r.BytesConsumed;
}
