using System.Text.Unicode;

namespace BinaryTool.Binary.Formatter;

public class AutoDetectFormatter : IFormatter
{
    private const string _desc = "auto detect";

    public string Description { get; private set; } = $"{_desc} (UTF-8 or Hex)";

    public string Format(ReadOnlySpan<byte> data)
    {
        var destination = (stackalloc char[data.Length]);
        var stat = Utf8.ToUtf16(data, destination, out _, out var written, false, false);

        if (stat == System.Buffers.OperationStatus.Done)
        {
            Description = $"{_desc} (UTF-8)";
            return new(destination[..written]);
        }
        else
        {
            Description = $"{_desc} (Hex)";
            return ConcatFormatter.SpaceSeparatedHex.Format(data);
        }
    }
}
