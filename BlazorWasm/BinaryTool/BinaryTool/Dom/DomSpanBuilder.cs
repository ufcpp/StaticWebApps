namespace BinaryTool.Dom;

internal class DomSpanBuilder
{
    private readonly List<DomSpan> _results = new();
    private readonly Stack<(object?, int startListIndex, int startPosition, string? key)> _hierarchy = new();
    private string? _key;

    public List<DomSpan> Results => _results;

    public void Key(string name) => _key = name;

    public void Add(object? value, Range range) => Add(DomKind.Value, _key, value, range);

    public void Exception(Exception value, Range range) => Add(DomKind.Error, _key ?? "$exception", value, range);

    private void Add(DomKind kind, string? key, object? value, Range range)
    {
        var parent = Parent();

        if (parent is Map m)
        {
            _results.Add(new(kind, range, value, key, 0));
            m[key!] = value;
        }
        else if (parent is List l)
        {
            _results.Add(new(kind, range, value, l.Count, 0));
            l.Add(value);
        }
        else
        {
            _results.Add(new(kind, range, value, null, 0));
        }

        _key = null;
    }

    private void Set(int index, DomKind kind, int length, string key, object? value, Range range)
    {
        var parent = Parent();

        if (parent is Map m)
        {
            _results[index] = new(kind, range, value, key, length);
            m.Add(key!, value);
        }
        else if (parent is List l)
        {
            _results[index] = new(kind, range, value, l.Count, length);
            l.Add(value);
        }
        else
        {
            _results[index] = new(kind, range, value, null, length);
        }
    }

    private object? Parent()
    {
        var (parent, _, _, _) = _hierarchy.Count > 0 ? _hierarchy.Peek() : default;
        return parent;
    }

    public void PushMap(int startPosition)
    {
        var obj = new Map();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition, _key));
    }

    public void PushList(int startPosition)
    {
        var obj = new List();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition, _key));
    }

    public void Pop(int endPosition)
    {
        var (obj, startListIndex, startPosition, name) = _hierarchy.Pop();

        if (obj is Map map) Set(startListIndex, DomKind.Map, map.Count, name!, map, startPosition..endPosition);
        else if (obj is List list) Set(startListIndex, DomKind.List, list.Count, name!, list, startPosition..endPosition);
    }

    public void PopAll(int endPosition)
    {
        while (_hierarchy.Count > 0) Pop(endPosition);
    }
}
