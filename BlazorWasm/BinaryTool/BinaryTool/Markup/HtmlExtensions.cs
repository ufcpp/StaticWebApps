using BinaryTool.Binary;
using System.Text;

namespace BinaryTool.Markup;

public static class HtmlExtensions
{
    public static string ToHtml(this Queue<Tag> tags, ReadOnlySpan<byte> data, bool isBinary)
    {
        var s = new StringBuilder();
        IFormatter f = isBinary ? Binary.Concat.SpaceSeparatedHex : Binary.Utf8.Instance;

        var start = 0;

        while (tags.TryDequeue(out var tag))
        {
            if (tag.Position != start)
            {
                s.Append(f.Format(data[start..tag.Position]));
            }

            if (tag.Kind is { } kind) s.Append($"""<span class="{kind}">""");
            else s.Append("</span>");

            start = tag.Position;
        }

        s.Append(f.Format(data[start..]));

        return s.ToString();
    }
}
