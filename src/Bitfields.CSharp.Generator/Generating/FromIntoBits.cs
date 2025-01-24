using System.Text;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class FromIntoBits
{
    /// <summary>
    ///     Generates the source code for the FromBits method.
    /// </summary>
    public static string FromBitsSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();

        source.AppendLine($$"""
                            {{bitfield.Visibility}} static {{bitfield.Name}} FromBits({{bitfield.Type.ToTypeString()}} bits)
                            {
                                var bitfield = new {{bitfield.Name}}();
                                {{SetFieldsFromBitsSource(bitfield, fields)}}
                                return bitfield;
                            }
                            """);
        return string.Join("\n", source);
    }
    
    public static string FromBitsWithDefaultsSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();

        source.AppendLine($$"""
                            {{bitfield.Visibility}} static {{bitfield.Name}} FromBitsWithDefaults({{bitfield.Type.ToTypeString()}} bits)
                            {
                                var bitfield = {{bitfield.Name}}.FromBits(bits);
                                {{GenerateFieldDefaultsImplSource(fields)}}
                                return bitfield;
                            }
                            """);
        return string.Join("\n", source);
    }
    
    private static string GenerateFieldDefaultsImplSource(List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveGetter))
            if (field.CustomType)
            {
                if (field.Default != null)
                    source.AppendLine($"bitfield.{field.Name} = {field.Default};");
            }
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
            {
                if (field.Default != null)
                    source.AppendLine($"bitfield.{field.Name} = {field.Default};");
            }
            else
            {
                if (field.Default != null)
                    source.AppendLine($"bitfield.{field.Name} = {field.Default};");
            }

        return string.Join("\n", source);
    }

    private static string SetFieldsFromBitsSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveGetter))
        {
            var fieldBitsConst = $"{field.Name.ToSnakeCase().ToUpper()}_BITS";
            var fieldOffsetConst = $"{field.Name.ToSnakeCase().ToUpper()}_OFFSET";
            var setterName = $"Set{field.Name.ToPascalCase()}";

            if (field.CustomType)
                source.AppendLine(
                    $"bitfield.{setterName}({field.Type}.FromBits(({field.CustomTypeFieldType})(bits >> {fieldOffsetConst} & ({field.CustomTypeFieldType})({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits})))));");
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
                source.AppendLine(
                    $"bitfield.{setterName}((((bits >> {fieldOffsetConst} & ({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))) != 0)));");
            else
                source.AppendLine(
                    $"bitfield.{setterName}(({field.Type})(bits >> {fieldOffsetConst} & ({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))));");
        }

        return string.Join("\n", source);
    }

    /// <summary>
    ///     Generates the source code for the ToBits method.
    /// </summary>
    public static string ToBitsSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        source.AppendLine($"{bitfield.Visibility} {bitfield.Type.ToTypeString()} ToBits() {{");
        var bitfieldType = bitfield.Type.ToTypeString();
        source.AppendLine($"{bitfieldType} bits = ({bitfieldType})0;");
        foreach (var field in fields)
            if (FieldUtil.DoesFieldHaveGetter(field))
            {
                var fieldBitsConst = $"{field.Name.ToSnakeCase().ToUpper()}_BITS";
                var fieldOffsetConst = $"{field.Name.ToSnakeCase().ToUpper()}_OFFSET";
                var getterName = $"Get{field.Name.ToPascalCase()}";

                if (field.CustomType)
                    source.AppendLine($"bits |= ({bitfieldType})(({bitfieldType}){getterName}().ToBits() << {fieldOffsetConst});");
                else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
                    source.AppendLine($"bits |= ({bitfieldType})(({bitfieldType})({getterName}() ? 1 : 0) << {fieldOffsetConst});");
                else if (field.Unsigned)
                {
                    source.AppendLine($"bits |= ({bitfieldType})(({bitfieldType}){getterName}() << {fieldOffsetConst});");
                }
                else
                {
                    source.AppendLine($"bits |= ({bitfieldType})((({bitfieldType}){getterName}() & ({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))) << {fieldOffsetConst});");
                }
            }
            else
            {
                if (field.CustomType)
                    source.AppendLine(
                        $"bits |= ({bitfieldType})((({bitfieldType}){field.Name}.ToBits() & ({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))) << {field.Offset});");
                else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
                    source.AppendLine($"bits |= ({bitfieldType})((({bitfieldType}){field.Name} ? 1 : 0) << {field.Offset});");
                else
                    source.AppendLine(
                        $"bits |= ({bitfieldType})((({bitfieldType}){field.Name} & ({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))) << {field.Offset});");
            }

        source.AppendLine("return bits;");
        source.AppendLine("}");
        return string.Join("\n", source);
    }
}