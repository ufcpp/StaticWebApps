using System.Text.Json;

namespace BinaryTool.Dom;

public class JsonLoader : ILoader
{
    public static readonly JsonLoader Instance = new();

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

        void addValue(object? value, Range range) => add(DomKind.Value, value, range);
        void addException(Exception value, Range range)
        {
            name = "$exception";
            add(DomKind.Error, value, range);
        }

        void add(DomKind kind, object? value, Range range)
        {
            if (parent is Map m)
            {
                results.Add(new(kind, range, value, name, 0));
                m.Add(name!, value);
            }
            else if (parent is List l)
            {
                results.Add(new(kind, range, value, l.Count, 0));
                l.Add(value);
            }
            else
            {
                results.Add(new(kind, range, value, null, 0));
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
            try
            {
                if (!r.Read()) break;
            }
            catch (JsonException ex)
            {
                addException(ex, (int)r.BytesConsumed..);
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
                    addValue(r.GetString(), r.Range());
                    break;
                case JsonTokenType.Number:
                    object? num = r.TryGetInt64(out var l) ? l : r.TryGetDouble(out var d) ? d : null;
                    addValue(num, r.Range());
                    break;
                case JsonTokenType.True:
                    addValue(true, r.Range());
                    break;
                case JsonTokenType.False:
                    addValue(false, r.Range());
                    break;
                case JsonTokenType.Null:
                    addValue(null, r.Range());
                    break;
            }
        }
    }
}

file static class Ex
{
    public static Range Range(this Utf8JsonReader r) => (int)r.TokenStartIndex..(int)r.BytesConsumed;
}
