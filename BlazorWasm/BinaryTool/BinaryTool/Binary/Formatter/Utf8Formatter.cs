using System.Text;

namespace BinaryTool.Binary.Formatter;

public class Utf8Formatter : IFormatter
{
    public static readonly Utf8Formatter Instance = new();

    public string Description => "as UTF-8";
    public string Format(ReadOnlySpan<byte> data) => Encoding.UTF8.GetString(data);
}
