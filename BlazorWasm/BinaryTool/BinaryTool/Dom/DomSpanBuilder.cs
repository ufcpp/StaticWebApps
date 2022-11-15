namespace BinaryTool.Dom;

internal class DomSpanBuilder
{
    private readonly List<DomSpan> _results = new();
    private readonly Stack<(object?, int startListIndex, int startPosition, object? key, int keyStart)> _hierarchy = new();
    private int _keyStart;
    private object? _key;

    public List<DomSpan> Results => _results;

    public void Key(object key, int position)
    {
        _key = key;
        _keyStart = position;
    }

    public void Add(object? value, (int start, int end) range) => Add(DomKind.Value, _key, _keyStart, value, range);

    public void Exception(Exception value, (int start, int end) range) => Add(DomKind.Error, _key ?? "$exception", _keyStart, value, range);

    private void Add(DomKind kind, object? key, int keyStart, object? value, (int start, int end) range)
    {
        var parent = Parent();

        var nextIndex = _results.Count + 1;
        if (parent is Map m)
        {
            _results.Add(new(kind, keyStart, range.start, range.end, key, value, nextIndex, 0));
            m[key!] = value;
        }
        else if (parent is List l)
        {
            _results.Add(new(kind, range.start, range.start, range.end, l.Count, value, nextIndex, 0));
            l.Add(value);
        }
        else
        {
            _results.Add(new(kind, range.start, range.start, range.end, null, value, nextIndex, 0));
        }

        _key = null;
    }

    private void Set(int index, DomKind kind, int length, object key, int keyStart, object? value, (int start, int end) range)
    {
        var parent = Parent();

        var nextIndex = _results.Count;
        if (parent is Map m)
        {
            _results[index] = new(kind, keyStart, range.start, range.end, key, value, nextIndex, length);

            if (key is not null) m.Add(key!, value);
        }
        else if (parent is List l)
        {
            _results[index] = new(kind, range.start, range.start, range.end, l.Count, value, nextIndex, length);
            l.Add(value);
        }
        else
        {
            _results[index] = new(kind, range.start, range.start, range.end, null, value, nextIndex, length);
        }
    }

    private object? Parent()
    {
        var (parent, _, _, _, _) = _hierarchy.Count > 0 ? _hierarchy.Peek() : default;
        return parent;
    }

    public void PushMap(int startPosition)
    {
        var obj = new Map();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition, _key, _keyStart));
    }

    public void PushList(int startPosition)
    {
        var obj = new List();
        var index = _results.Count;
        _results.Add(default!);

        _hierarchy.Push(new(obj, index, startPosition, _key, _keyStart));
    }

    public void Pop(int endPosition)
    {
        var (obj, startListIndex, startPosition, key, keyStart) = _hierarchy.Pop();

        if (obj is Map map) Set(startListIndex, DomKind.Map, map.Count, key!, keyStart, map, (startPosition, endPosition));
        else if (obj is List list) Set(startListIndex, DomKind.List, list.Count, key!, keyStart, list, (startPosition, endPosition));
    }

    public void PopAll(int endPosition)
    {
        while (_hierarchy.Count > 0) Pop(endPosition);
    }
}
