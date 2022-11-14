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

            Add(tags, span.KeyStart, span.Start, span.End, nextStart, span.Kind.ToString());
        }

        tags.Sort();
        return new(tags);
    }

    private static void Add(List<Tag> tags, int keyStart, int start, int end, int nextStart, string kind)
    {
        Add(tags, keyStart, nextStart, 1, $"node {kind}");
        if (keyStart != start) Add(tags, keyStart, start, 0, "key");
        Add(tags, start, end, 0, "body");
        if (end != nextStart) Add(tags, end, nextStart, 0, "trail");
    }

    private static void Add(List<Tag> tags, int start, int end, int priority, string kind)
    {
        var info = new TagInfo(start, end, priority, kind);
        tags.Add(info.Open());
        tags.Add(info.Close());
    }
}
