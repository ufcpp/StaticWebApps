using System.Text.Unicode;

namespace BinaryTool.Binary.Formatter;

public class AutoDetectFormatter : IFormatter
{
    public static readonly AutoDetectFormatter Instance = new();

    public string Description => "auto detect (UTF-8 or Hex)";

    public string Format(ReadOnlySpan<byte> data)
    {
        var destination = (stackalloc char[data.Length]);
        var stat = Utf8.ToUtf16(data, destination, out _, out var written, false, false);

        if (stat == System.Buffers.OperationStatus.Done) return new(destination[..written]);

        return ConcatFormatter.SpaceSeparatedHex.Format(data);
    }
}
