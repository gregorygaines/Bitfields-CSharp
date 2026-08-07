using System.Globalization;
using Microsoft.CodeAnalysis;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Parsing.Bitflag;

public static class BitflagParser
{
    private const string BitflagAttributeName = "Bitfields.CSharp.Bitflag.BitflagAttribute";
    private const string BaseAttributeName = "Bitfields.CSharp.Bitflag.BaseAttribute";

    public static ParsedBitflag Parse(INamedTypeSymbol symbol, AttributeData attribute)
    {
        var type = (int)attribute.ConstructorArguments[0].Value! switch
        {
            0 => InternalTypeUtil.InternalType.Byte,
            1 => InternalTypeUtil.InternalType.Char,
            2 => InternalTypeUtil.InternalType.UShort,
            3 => InternalTypeUtil.InternalType.UInt,
            4 => InternalTypeUtil.InternalType.ULong,
            5 => InternalTypeUtil.InternalType.UInt128,
            _ => throw new ArgumentOutOfRangeException()
        };
        var fields = symbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsConst && f.HasConstantValue).ToList();
        var selected = fields.FirstOrDefault(HasBaseAttribute);
        if (selected == null) throw new InvalidOperationException($"Bitflag '{symbol.Name}' must have a [Base] member.");

        return new ParsedBitflag
        {
            Name = symbol.Name,
            FullyQualifiedName = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Namespace = symbol.ContainingNamespace.IsGlobalNamespace ? "" : symbol.ContainingNamespace.ToDisplayString(),
            Visibility = symbol.DeclaredAccessibility == Accessibility.Public ? "public" : "internal",
            Type = type,
            Variants = fields.Select(f => new ParsedBitflagVariant { Name = f.Name, Value = FormatValue(f.ConstantValue!, type) }).ToList(),
            BaseValue = FormatValue(selected.ConstantValue!, type)
        };
    }

    public static bool IsBitflagAttribute(AttributeData attribute) => attribute.AttributeClass?.ToDisplayString() == BitflagAttributeName;
    private static bool HasBaseAttribute(IFieldSymbol field) => field.GetAttributes().Any(a => a.AttributeClass?.ToDisplayString() == BaseAttributeName);

    private static string FormatValue(object value, InternalTypeUtil.InternalType type)
    {
        var text = Convert.ToString(value, CultureInfo.InvariantCulture)!;
        return type == InternalTypeUtil.InternalType.ULong ? text + "UL" : text;
    }
}
