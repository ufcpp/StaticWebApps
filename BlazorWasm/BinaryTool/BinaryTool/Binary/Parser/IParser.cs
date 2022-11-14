using System.Collections.Immutable;

namespace BinaryTool.Binary.Parser;

public interface IParser : IDescriptiveItem
{
    byte[] Parse(string input);

    public static readonly ImmutableArray<IParser> Defaults = ImmutableArray.Create(new IParser[]
    {
        HexParser.Instance,
        ConsecutiveHexParser.Instance,
        DecimalParser.Instance,
        Base64Parser.Instance,
        Utf8Parser.Instance,
    });

    public static ImmutableArray<IParser> WithAutoDetect() => ImmutableArray.CreateRange(
        Defaults.Prepend(new AutoDetectParser()));
}

//todo: C# style?
// 123, 0x12, 0b11, 1_23, 0x1_2, 0b1111_1111, ...
