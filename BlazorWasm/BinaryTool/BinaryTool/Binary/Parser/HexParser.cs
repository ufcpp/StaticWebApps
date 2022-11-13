namespace BinaryTool.Binary.Parser;

public class HexParser : IParser
{
    public static readonly HexParser Instance = new();

    public string Description => "hexadecimal numbers";
    public byte[] Parse(string input) => ParserHelper.Parse(input, true);
}
