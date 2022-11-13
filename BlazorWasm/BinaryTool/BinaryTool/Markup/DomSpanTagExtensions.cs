using BinaryTool.Dom;

namespace BinaryTool.Markup;

/// <summary>
/// Create <see cref="Tag"/>s from <see cref="DomSpan"/>s.
/// </summary>
public static class DomSpanTagExtensions
{
    private static Tag Open(DomSpan span, int dataLength) => new(span.Range.Start.GetOffset(dataLength), span.Range.End.GetOffset(dataLength), span.Kind.ToString());
    private static Tag Close(DomSpan span, int dataLength) => new(span.Range.End.GetOffset(dataLength), span.Range.Start.GetOffset(dataLength), null);

    public static Queue<Tag> Build(this IReadOnlyList<DomSpan> spans, int dataLength)
    {
        var tags = new List<Tag>();

        foreach (var span in spans)
        {
            //todo nest level
            tags.Add(Open(span, dataLength));
            tags.Add(Close(span, dataLength));
        }

        tags.Sort();
        return new(tags);
    }
}
