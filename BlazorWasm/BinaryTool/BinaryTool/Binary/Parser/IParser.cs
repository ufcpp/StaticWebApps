using System.Collections.Immutable;

namespace BinaryTool.Binary.Parser;

public interface IParser : IDescriptiveItem
{
    byte[] Parse(string input);

    public static readonly ImmutableArray<IParser> DefaultParsers = ImmutableArray.Create(new IParser[]
    {
        AutoDetectParser.Instance,
        HexParser.Instance,
        ConsecutiveHexParser.Instance,
        DecimalParser.Instance,
        Base64Parser.Instance,
        Utf8Parser.Instance,
    });
}

//todo: C# style?
// 123, 0x12, 0b11, 1_23, 0x1_2, 0b1111_1111, ...
