namespace BinaryTool.Dom;

public record DomSpan(DomKind Kind, Range Range, object? Value, object? Key, int Length);
