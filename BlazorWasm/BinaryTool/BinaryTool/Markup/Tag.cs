namespace BinaryTool.Markup;

public readonly record struct Tag(int Position, int OppositePosition, string? Kind) : IComparable<Tag>
{
    public bool IsClose => Kind is null;

    public int CompareTo(Tag other)
    {
        var pos = Position.CompareTo(other.Position);
        if (pos != 0) return pos;

        // close tags must preceed open tags.
        if (IsClose && !other.IsClose) return -1;
        if (!IsClose && other.IsClose) return 1;

        // reverse order for opposite tags.
        return -OppositePosition.CompareTo(other.OppositePosition);
    }
}
