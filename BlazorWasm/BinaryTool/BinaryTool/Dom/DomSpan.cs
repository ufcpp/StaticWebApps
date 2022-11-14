namespace BinaryTool.Dom;

public record DomSpan(DomKind Kind, int KeyStart, int Start, int End, object? Key, object? Value, int NextIndex, int Length);
