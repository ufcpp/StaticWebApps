using System.Text;

namespace BinaryTool.Binary.Parser;

public class Utf8Parser : IParser
{
    public static readonly Utf8Parser Instance = new();

    public string Description => "UTF-8";
    public byte[] Parse(string input) => Encoding.UTF8.GetBytes(input);
}
