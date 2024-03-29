using MessagePack;

namespace BinaryTool.Dom.Reader;

public class MessagePackReader : IReader
{
    public static readonly MessagePackReader Instance = new();

    public string Description => "MessagePack";

    public (List<DomSpan> spans, bool isBinary) Read(byte[] data)
    {
        var reader = new MessagePack.MessagePackReader(data);
        var builder = new DomSpanBuilder();
        Parse(ref reader, builder);
        builder.PopAll((int)reader.Consumed);
        return (builder.Results, true);
    }

    private static void Parse(ref MessagePack.MessagePackReader r, DomSpanBuilder builder)
    {
        var stack = new Stack<(int, bool, bool)>();
        (int count, bool isMap, bool isKey) current = default;

        void push(int count, bool isMap)
        {
            var c = (count, isMap, false);
            stack.Push(current);
            current = c;
        }

        void decl(int pos)
        {
            --current.count;
            if (current.count == 0)
            {
                pop(pos);
            }
        }

        void pop(int pos)
        {
            builder.Pop(pos);
            current = stack!.Pop();
            decl(pos);
        }

        void add<T>(int s, T v, long e)
        {
            object? obj = typeof(T) == typeof(Nil) ? null : v;

            if (current.isMap && current.isKey)
            {
                builder.Key(obj!, s);
            }
            else
            {
                if (v is Exception ex) builder.Exception(ex, (s, (int)e));
                else builder.Add(obj, (s, (int)e));
                decl((int)e);
            }
        }

        int s = 0;
        try
        {
            while (!r.End)
            {
                s = (int)r.Consumed;
                switch (r.NextMessagePackType)
                {
                    case MessagePackType.Unknown:
                        break;
                    case MessagePackType.Integer:
                        add(s, r.ReadInt64(), r.Consumed);
                        break;
                    case MessagePackType.Nil:
                        add(s, r.ReadNil(), r.Consumed);
                        break;
                    case MessagePackType.Boolean:
                        add(s, r.ReadBoolean(), r.Consumed);
                        break;
                    case MessagePackType.Float:
                        add(s, r.ReadDouble(), r.Consumed);
                        break;
                    case MessagePackType.String:
                        add(s, r.ReadString(), r.Consumed);
                        break;
                    case MessagePackType.Binary:
                        add(s, r.ReadBytes(), r.Consumed);
                        break;
                    case MessagePackType.Array:
                        {
                            r.TryReadArrayHeader(out var c);
                            push(c, false);
                            builder.PushList(s);
                            if (c == 0) pop((int)r.Consumed);
                        }
                        break;
                    case MessagePackType.Map:
                        {
                            r.TryReadMapHeader(out var c);
                            push(c, true);
                            builder.PushMap(s);
                            if (c == 0) pop((int)r.Consumed);
                        }
                        break;
                    case MessagePackType.Extension:
                        add(s, r.ReadDateTime(), r.Consumed);
                        break;
                    default:
                        break;
                }

                if (current.isMap)
                    current.isKey = !current.isKey;
            }
        }
        catch (Exception ex)
        {
            builder.Exception(ex, (s, (int)r.Sequence.Length));
            return;
        }
    }
}
