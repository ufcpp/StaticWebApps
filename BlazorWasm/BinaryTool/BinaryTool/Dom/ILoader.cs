namespace BinaryTool.Dom;

public interface ILoader : IDescriptiveItem
{
    List<DomSpan> Parse(byte[] data);
}
