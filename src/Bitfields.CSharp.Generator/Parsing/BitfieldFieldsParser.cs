using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Bitfields.CSharp.Generator.Parsing;

public static class BitfieldFieldsParser
{
    private const string PaddingFieldSuffix = "_";

    private const string BitsAttributeName = "bits";
    private const string CustomFieldTypeAttributeName = "customFieldType";
    private const string CustomFieldBaseAttributeName = "customFieldBase";

    private const int BitsArgumentIndex = 0;
    private const int CustomFieldTypeArgumentIndex = 1;
    private const int CustomFieldBaseArgumentIndex = 2;

    public static ParseResults ParseBitfieldFields(ParsedBitfield bitfield,
        List<MemberDeclarationSyntax> fieldDeclaration)
    {
        List<ParsedBitfieldField> fields = [];
        foreach (var field in fieldDeclaration)
        {
            var result = ParseBitfieldField(bitfield, field, fields);
            if (result.Error != null)
                return new ParseResults
                {
                    Error = result.Error
                };
            fields.Add(result.Field);
        }

        return new ParseResults
        {
            Fields = fields
        };
    }

    private static InternalParseResult ParseBitfieldField(ParsedBitfield bitfield,
        MemberDeclarationSyntax fieldDeclaration, List<ParsedBitfieldField> prevFields)
    {
        var typeStr = GetMemberType(fieldDeclaration);
        if (InternalTypeUtil.IsUnsupportedFieldType(typeStr))
            return new InternalParseResult
            {
                Error = new ParseError
                {
                    Descriptor = DiagnosticDescriptors.Create(
                        "BITFS002",
                        "Unsupported bitfield field type",
                        $"The field type '{typeStr}' is not supported for bitfields. Only integer and boolean types are supported."),
                    Location = fieldDeclaration.GetLocation()
                }
            };

        var isCustomType = !InternalTypeUtil.IsPrimitiveType(typeStr);
        var bitsAttribute = ParseBitsAttribute(fieldDeclaration);
        var name = GetMemberName(fieldDeclaration);
        var defaultVal = GetDefaultValue(fieldDeclaration);
        var isEnum = false;

        int? bits;
        string customTypeFieldType = null;
        if (isCustomType)
        {
            if (bitsAttribute == null || bitsAttribute.Bits == null || bitsAttribute.CustomFieldNamespaceType == null)
                return new InternalParseResult
                {
                    Error = new ParseError
                    {
                        Descriptor = DiagnosticDescriptors.Create(
                            "BITFS003",
                            "Custom and nested type fields must have a [Bits] attribute with defined bits and field type",
                            "Custom and nested type fields must have a [Bits] attribute with defined bits and field type, otherwise we can't determine the size or type of the field."),
                        Location = fieldDeclaration.GetLocation()
                    }
                };

            bits = bitsAttribute.Bits;
            customTypeFieldType = bitsAttribute.CustomFieldNamespaceType;
            isEnum = bitsAttribute.IsEnum;
        }
        else
        {
            var internalType = InternalTypeUtil.GetInternalType(typeStr);
            bits = bitsAttribute != null && bitsAttribute.Bits != null
                ? bitsAttribute.Bits
                : InternalTypeUtil.GetBitsFromInternalType((InternalTypeUtil.InternalType)internalType!);
            isEnum = bitsAttribute != null && bitsAttribute.IsEnum;

            var internalTypeBits =
                InternalTypeUtil.GetBitsFromInternalType((InternalTypeUtil.InternalType)internalType!);
            if (bits > internalTypeBits)
                return new InternalParseResult
                {
                    Error = new ParseError
                    {
                        Descriptor = DiagnosticDescriptors.Create(
                            "BITFS004",
                            "Field type is too small to hold the specified bits",
                            $"The field type '{typeStr}' ({internalTypeBits} bits) is too small to hold the specified bits '{bits} bits'."),
                        Location = fieldDeclaration.GetLocation()
                    }
                };
        }

        if (bits <= 0)
            return new InternalParseResult
            {
                Error = new ParseError
                {
                    Descriptor = DiagnosticDescriptors.Create(
                        "BITFS005",
                        "Field bits must be greater than 0",
                        "Field bits must be greater than 0."),
                    Location = fieldDeclaration.GetLocation()
                }
            };

        var offset = CalculateOffset(bits!.Value, bitfield, prevFields);
        var unsigned = !isCustomType &&
                       InternalTypeUtil.IsInternalTypeUnsigned(
                           (InternalTypeUtil.InternalType)InternalTypeUtil.GetInternalType(typeStr)!);
        var padding = name.EndsWith(PaddingFieldSuffix);

        var parsedField = new ParsedBitfieldField
        {
            Name = name,
            Type = typeStr,
            CustomType = isCustomType,
            CustomTypeFieldType = customTypeFieldType!,
            Visibility = "public",
            Bits = bits.Value,
            Offset = offset,
            Default = defaultVal,
            Unsigned = unsigned,
            Padding = padding,
            IsEnum = isEnum
        };

        return new InternalParseResult
        {
            Field = parsedField
        };
    }

    private static string GetMemberType(MemberDeclarationSyntax member)
    {
        return member switch
        {
            FieldDeclarationSyntax field => field.Declaration.Type.ToString(),
            PropertyDeclarationSyntax property => property.Type.ToString(),
            MethodDeclarationSyntax method => method.ReturnType.ToString(),
            EventDeclarationSyntax @event => @event.Type.ToString(),
            _ => "Unknown"
        };
    }

    private static string GetMemberName(MemberDeclarationSyntax member)
    {
        return member switch
        {
            FieldDeclarationSyntax field => field.Declaration.Variables.First().Identifier.Text,
            PropertyDeclarationSyntax property => property.Identifier.Text,
            MethodDeclarationSyntax method => method.Identifier.Text,
            EventDeclarationSyntax @event => @event.Identifier.Text,
            _ => "Unknown"
        };
    }

    private static string? GetDefaultValue(MemberDeclarationSyntax fieldDeclaration)
    {
        string? value = null;
        switch (fieldDeclaration)
        {
            case FieldDeclarationSyntax field:
                value = field.Declaration.Variables.First().Initializer?.Value.ToString();
                break;

            case PropertyDeclarationSyntax property:
                value = property.Initializer?.Value.ToString();
                break;
        }

        return value ?? null;
    }

    private static InternalBitsAttribute? ParseBitsAttribute(MemberDeclarationSyntax fieldDeclaration)
    {
        if (fieldDeclaration.AttributeLists.Count == 0) return null;

        var bitsAttribute = GetBitsAttribute(fieldDeclaration);
        if (bitsAttribute == null) return null;

        var internalBitsAttribute = new InternalBitsAttribute();
        if (bitsAttribute.ArgumentList == null) return internalBitsAttribute;

        for (var arg = 0; arg < bitsAttribute.ArgumentList.Arguments.Count; arg++)
        {
            var argument = bitsAttribute.ArgumentList.Arguments[arg];

            if (argument.ToString().Contains(':'))
            {
                var name = argument.ToString().Split(':')[0];
                var value = argument.ToString().Split(':')[1];
                switch (name)
                {
                    case BitsAttributeName:
                        internalBitsAttribute.Bits = int.Parse(value);
                        break;
                    case CustomFieldTypeAttributeName:
                        switch (value)
                        {
                            case "CustomFieldType.Byte":
                                internalBitsAttribute.CustomFieldNamespaceType = "byte";
                                break;
                            case "CustomFieldType.SByte":
                                internalBitsAttribute.CustomFieldNamespaceType = "sbyte";
                                break;
                            case "CustomFieldType.UShort":
                                internalBitsAttribute.CustomFieldNamespaceType = "ushort";
                                break;
                            case "CustomFieldType.Short":
                                internalBitsAttribute.CustomFieldNamespaceType = "short";
                                break;
                            case "CustomFieldType.UInt":
                                internalBitsAttribute.CustomFieldNamespaceType = "uint";
                                break;
                            case "CustomFieldType.Int":
                                internalBitsAttribute.CustomFieldNamespaceType = "int";
                                break;
                            case "CustomFieldType.ULong":
                                internalBitsAttribute.CustomFieldNamespaceType = "ulong";
                                break;
                            case "CustomFieldType.Long":
                                internalBitsAttribute.CustomFieldNamespaceType = "long";
                                break;
                        }
                        break;
                    case CustomFieldBaseAttributeName:
                        internalBitsAttribute.IsEnum = value == "CustomFieldBase.Enum";
                        break;
                    default:
                        throw new Exception($"Invalid attribute argument: {name}");
                }
            }
            else
            {
                var value = argument.Expression.GetFirstToken().Value;
                var fullEnum = "";
                switch (arg)
                {
                    case BitsArgumentIndex:
                        internalBitsAttribute.Bits = (int)value!;
                        break;
                    case CustomFieldTypeArgumentIndex:
                        fullEnum = argument.Expression.ToFullString();
                        switch (fullEnum)
                        {
                            case "CustomFieldType.Byte":
                                internalBitsAttribute.CustomFieldNamespaceType = "byte";
                                break;
                            case "CustomFieldType.SByte":
                                internalBitsAttribute.CustomFieldNamespaceType = "sbyte";
                                break;
                            case "CustomFieldType.UShort":
                                internalBitsAttribute.CustomFieldNamespaceType = "ushort";
                                break;
                            case "CustomFieldType.Short":
                                internalBitsAttribute.CustomFieldNamespaceType = "short";
                                break;
                            case "CustomFieldType.UInt":
                                internalBitsAttribute.CustomFieldNamespaceType = "uint";
                                break;
                            case "CustomFieldType.Int":
                                internalBitsAttribute.CustomFieldNamespaceType = "int";
                                break;
                            case "CustomFieldType.ULong":
                                internalBitsAttribute.CustomFieldNamespaceType = "ulong";
                                break;
                            case "CustomFieldType.Long":
                                internalBitsAttribute.CustomFieldNamespaceType = "long";
                                break;
                            default:
                                throw new Exception($"Invalid custom field type: {fullEnum}");
                        }
                        break;
                    case CustomFieldBaseArgumentIndex:
                        fullEnum = argument.Expression.ToFullString();
                        if (value != null) internalBitsAttribute.IsEnum = fullEnum == "CustomFieldBase.Enum";
                        break;
                    default:
                        throw new Exception($"Invalid attribute argument index: {arg}");
                }
            }
        }

        return internalBitsAttribute;
    }

    private static AttributeSyntax? GetBitsAttribute(MemberDeclarationSyntax fieldDeclaration)
    {
        if (fieldDeclaration.AttributeLists.Count == 0) return null;
        return fieldDeclaration.AttributeLists.SelectMany(list => list.Attributes)
            .First(attribute => attribute.Name.GetText().ToString() == "Bits");
    }

    /// <summary>
    ///     Calculate the offset of a field based on previous fields.
    /// </summary>
    private static int CalculateOffset(int bits, ParsedBitfield bitfield, List<ParsedBitfieldField> prevFields)
    {
        var offset = prevFields.Select(f => f.Bits).Sum();
        //   throw new Exception("Calculating offset from bits");

        if (bitfield.Order == ParsedBitfield.BitOrder.Lsb) return offset;

        var bitfieldTypeBits = InternalTypeUtil.GetBitsFromInternalType(bitfield.Type);

        // We calculate offset starting from the left. There's a chance that
        // the total bits of all fields is greater than the number of bits
        // of the bitfield type. We will catch it later so
        // we can ignore for now.
        if (offset + bits < bitfieldTypeBits) return bitfieldTypeBits - bits - offset;

        // We've underflow the bitfield type, this will be caught later.
        return 0;
    }

    public class ParseResults
    {
        public List<ParsedBitfieldField> Fields { get; set; }
        public ParseError? Error { get; set; }
    }

    private class InternalParseResult
    {
        public ParsedBitfieldField Field { get; set; }
        public ParseError? Error { get; set; }
    }

    public class ParseError
    {
        public DiagnosticDescriptor Descriptor { get; set; }
        public Location Location { get; set; }
    }
    
    private class InternalBitsAttribute
    {
        public int? Bits { get; set; }
        public string? CustomFieldNamespaceType { get; set; }
        public bool IsEnum { get; set; }
    }
}