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
        CsharpIngeterLiteralParser.Instance,
    });

    public static ImmutableArray<IParser> WithAutoDetect() => ImmutableArray.CreateRange(
        Defaults.Prepend(new AutoDetectParser()));
}
