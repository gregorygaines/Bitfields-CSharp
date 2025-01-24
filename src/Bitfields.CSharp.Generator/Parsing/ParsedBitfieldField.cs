namespace Bitfields.CSharp.Generator.Parsing;

public class ParsedBitfieldField
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Visibility { get; set; }
    public int Bits { get; set; }
    public int Offset { get; set; }
    public bool Unsigned { get; set; }
    public bool Padding { get; set; }
    public bool CustomType { get; set; }
    public string CustomTypeFieldType { get; set; }
    public string? Default { get; set; }
}