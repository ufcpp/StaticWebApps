namespace BinaryTool.Dom;

internal class DomSpanBuilder
{
    private readonly List<DomSpan> _results = new();
    private readonly Stack<(object?, int startListIndex, int startPosition)> _hierarchy = new();
    private string? _name;

    public List<DomSpan> Results => _results;

    public void Name(string name) => _name = name;

    public void Add(object? value, Range range) => Add(DomKind.Value, _name, value, range);

    public void Exception(Exception value, Range range) => Add(DomKind.Error, _name ?? "$exception", value, range);

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

        _name = null;
    }

    private void Set(int index, DomKind kind, int length, object? value, Range range)
    {
        var parent = Parent();

        if (parent is Map m)
        {
            _results[index] = new(kind, range, value, _name, length);
            m.Add(_name!, value);
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
        var (parent, _, _) = _hierarchy.Count > 0 ? _hierarchy.Peek() : default;
        return parent;
    }

    public void PushMap(int startPosition)
    {
        var obj = new Map();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition));
    }

    public void PushList(int startPosition)
    {
        var obj = new List();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition));
    }

    public void Pop(int endPosition)
    {
        var (obj, startListIndex, startPosition) = _hierarchy.Pop();

        if (obj is Map map) Set(startListIndex, DomKind.Map, map.Count, map, startPosition..endPosition);
        else if (obj is List list) Set(startListIndex, DomKind.List, list.Count, list, startPosition..endPosition);
    }

    public void PopAll(int endPosition)
    {
        while (_hierarchy.Count > 0) Pop(endPosition);
    }
}
