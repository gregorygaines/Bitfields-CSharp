namespace Bitfields.CSharp.Generator.Parsing;

public class ParsedBitfield
{
    public enum BitOrder
    {
        Lsb,
        Msb
    }

    public string Name { get; set; }
    public BitOrder Order { get; set; }
    public InternalTypeUtil.InternalType Type { get; set; }
    public string Visibility { get; set; }
}