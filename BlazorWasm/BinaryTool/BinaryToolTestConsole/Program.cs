// See https://aka.ms/new-console-template for more information
using BinaryTool.Binary.Parser;
using BinaryTool.Dom.Reader;

#if tru
var source = """
    86 FF C3 00 81 00 91 92 01 83 00 82 00 01 01 02 01 A2 61 31 02 03 01 81 00 92 92 01 80 92 02 80 02 81 00 91 92 01 80 03 81 00 01 04 81 00 CE 6F 0C C7 85 
    """;
#else
var source = """
    82
      01 92 01 80
      02 92 02 80
    """;
#endif

var parser = new HexParser();
var dat = parser.Parse(source);

var reader = new MessagePackReader();
var (list, _) = reader.Read(dat);

foreach (var item in list)
{
    Console.WriteLine(item);
}
