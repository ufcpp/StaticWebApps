namespace BinaryTool.Markup;


public record TagInfo(int Position, int OppositePosition, int Priority, string Name)
{
    public Tag Open() => new(false, this);
    public Tag Close() => new(true, this);
}

public readonly record struct Tag(bool IsClose, TagInfo Info) : IComparable<Tag>
{
    public string Name() => Info.Name;
    public int Position() => IsClose ? Info.OppositePosition : Info.Position;
    private int OppositePosition() => IsClose ? Info.Position : Info.OppositePosition; 

    public int CompareTo(Tag other)
    {
        var pos = Position().CompareTo(other.Position());
        if (pos != 0) return pos;

        // close tags must preceed open tags.
        if (IsClose && !other.IsClose) return -1;
        if (!IsClose && other.IsClose) return 1;

        // reverse order for opposite tags.
        var opp = -OppositePosition().CompareTo(other.OppositePosition());
        if(opp != 0) return opp;

        var p = Info.Priority.CompareTo(other.Info.Priority);
        if (!IsClose) p = -p;
        return p;
    }
}
