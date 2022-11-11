namespace BinaryTool.Dom;

public interface ILoader
{
    string Description { get; }
    List<DomSpan> Parse(byte[] data);
}
