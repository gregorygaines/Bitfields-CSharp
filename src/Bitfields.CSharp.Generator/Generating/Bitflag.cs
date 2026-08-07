using System.Text;
using Bitfields.CSharp.Generator.Parsing.Bitflag;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class Bitflag
{
    public static string Generate(ParsedBitflag bitflag)
    {
        var type = bitflag.Type.ToTypeString();
        var source = new StringBuilder();
        source.AppendLine($"{bitflag.Visibility} static class {bitflag.Name}Extensions {{");
        source.AppendLine($"    {bitflag.Visibility} static {bitflag.FullyQualifiedName} FromBits({type} bits) => bits switch {{");
        foreach (var variant in bitflag.Variants)
            source.AppendLine($"        {variant.Value} => {bitflag.FullyQualifiedName}.{variant.Name},");
        source.AppendLine($"        _ => {bitflag.FullyQualifiedName}.{FindName(bitflag)}");
        source.AppendLine("    };");
        source.AppendLine($"    {bitflag.Visibility} static {bitflag.FullyQualifiedName} FromBits(this {bitflag.FullyQualifiedName} _, {type} bits) => FromBits(bits);");
        source.AppendLine($"    {bitflag.Visibility} static {type} ToBits(this {bitflag.FullyQualifiedName} value) => ({type})value;");
        source.AppendLine("}");
        return source.ToString();
    }

    private static string FindName(ParsedBitflag bitflag)
    {
        var baseValue = bitflag.Variants.First(v => v.Value == bitflag.BaseValue);
        return baseValue.Name;
    }
}
