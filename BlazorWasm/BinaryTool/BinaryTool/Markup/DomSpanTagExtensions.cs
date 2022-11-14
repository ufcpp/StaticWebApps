using BinaryTool.Dom;

namespace BinaryTool.Markup;

/// <summary>
/// Create <see cref="Tag"/>s from <see cref="DomSpan"/>s.
/// </summary>
public static class DomSpanTagExtensions
{
    public static Queue<Tag> Build(this IReadOnlyList<DomSpan> spans, int dataLength)
    {
        var tags = new List<Tag>();

        foreach (var span in spans)
        {
            var next = span.NextIndex;
            var nextStart = next < spans.Count ? spans[next].KeyStart : dataLength;

            Add(tags, span.KeyStart, span.Start, span.End, nextStart, ToClassName(span.Kind));
        }

        tags.Sort();
        return new(tags);
    }

    private static string ToClassName(DomKind kind) => kind switch
    {
        DomKind.Map => "map",
        DomKind.List => "list",
        DomKind.Error => "error",
        _ => "other",
    };

    private static void Add(List<Tag> tags, int keyStart, int start, int end, int nextStart, string name)
    {
        Add(tags, keyStart, nextStart, 1, $"node {name}");
        if (keyStart != start) Add(tags, keyStart, start, 0, "key");
        Add(tags, start, end, 0, "body");
        if (end != nextStart) Add(tags, end, nextStart, 0, "trail");
    }

    private static void Add(List<Tag> tags, int start, int end, int priority, string name)
    {
        var info = new TagInfo(start, end, priority, name);
        tags.Add(info.Open());
        tags.Add(info.Close());
    }
}
