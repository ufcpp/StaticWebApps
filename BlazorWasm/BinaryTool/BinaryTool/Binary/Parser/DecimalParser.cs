namespace BinaryTool.Binary.Parser;

public class DecimalParser : IParser
{
    public static readonly DecimalParser Instance = new();

    public string Description => "decimal numbers";
    public byte[] Parse(string input) => ParserHelper.Parse(input, false);
}
