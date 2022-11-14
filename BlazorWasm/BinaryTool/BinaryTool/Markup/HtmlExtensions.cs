using BinaryTool.Binary.Formatter;
using System.Text;

namespace BinaryTool.Markup;

public static class HtmlExtensions
{
    private static readonly IFormatter _bin = new ConcatFormatter("", " ", "", NumberFormat.Hex, true);
    private static readonly IFormatter _str = Utf8Formatter.Instance;

    public static string ToHtml(this Queue<Tag> tags, ReadOnlySpan<byte> data, bool isBinary)
    {
        var s = new StringBuilder();
        var f = isBinary ? _bin : _str;

        var start = 0;

        while (tags.TryDequeue(out var tag))
        {
            var pos = tag.Position();
            if (pos != start)
            {
                s.Append(f.Format(data[start..pos]));
            }

            if (!tag.IsClose) s.Append($"""<span class="{tag.Name()}">""");
            else s.Append("</span>");

            start = pos;
        }

        s.Append(f.Format(data[start..]));

        return s.ToString();
    }
}
