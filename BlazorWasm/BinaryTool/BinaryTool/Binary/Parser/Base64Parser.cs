namespace BinaryTool.Binary.Parser;

public class Base64Parser : IParser
{
    public static readonly Base64Parser Instance = new();

    public string Description => "base64";
    public byte[] Parse(string input)
    {
        try
        {
            return Convert.FromBase64String(input);
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }
}
