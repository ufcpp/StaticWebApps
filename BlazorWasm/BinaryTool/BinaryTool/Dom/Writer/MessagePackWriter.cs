namespace BinaryTool.Dom.Writer;

public class MessagePackWriter : IWriter
{
    public static readonly MessagePackWriter Instance = new();

    public string Description => "MessagePack";

    public byte[] Write(IReadOnlyList<DomSpan> spans)
    {
        if (spans.Count == 0) return Array.Empty<byte>();
        if (spans[0].Value is Exception) return Array.Empty<byte>();
        return MessagePack.MessagePackSerializer.Serialize(spans[0].Value);
    }
}
