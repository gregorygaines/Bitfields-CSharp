namespace Bitfields.CSharp.Generator.Parsing.Bitflag;

public class ParsedBitflag
{
    public string Name { get; set; } = "";
    public string FullyQualifiedName { get; set; } = "";
    public string Namespace { get; set; } = "";
    public string Visibility { get; set; } = "internal";
    public InternalTypeUtil.InternalType Type { get; set; }
    public List<ParsedBitflagVariant> Variants { get; set; } = [];
    public string BaseValue { get; set; } = "";
}

public class ParsedBitflagVariant
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}
