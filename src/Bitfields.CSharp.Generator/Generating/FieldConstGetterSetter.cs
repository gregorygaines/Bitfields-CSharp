using System.Text;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class FieldConstGetterSetter
{
    /// <summary>
    ///     Generates the source code for the field constants.
    /// </summary>
    public static string GenerateFieldConstantsSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(field =>
                     FieldUtil.DoesFieldHaveGetter(field) || FieldUtil.DoesFieldHaveSetter(field)))
        {
            source.AppendLine(
                $"{bitfield.Visibility} const int {field.Name.ToSnakeCase().ToUpper()}_BITS = {field.Bits};");
            source.AppendLine(
                $"{bitfield.Visibility} const int {field.Name.ToSnakeCase().ToUpper()}_OFFSET = {field.Offset};");
        }

        return string.Join("\n", source);
    }

    /// <summary>
    ///     Generates the source code for the field getters.
    /// </summary>
    public static string GenerateFieldGettersSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveGetter))
        {
            var getterName = $"Get{field.Name.ToPascalCase()}";
            var fieldBitsConst = $"{field.Name.ToSnakeCase().ToUpper()}_BITS";

            if (field.CustomType)
            {
                if (field.IsEnum)
                {
                    source.AppendLine($"""
                                       {bitfield.Visibility} {field.Type} {getterName}() => 
                                          {field.Type}.{Common.CustomFieldEnumBaseEnumName}.FromBits(({field.CustomTypeFieldType})({field.Name}.ToBits() & ({field.CustomTypeFieldType})({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))));
                                       """);
                }
                else
                {
                    source.AppendLine($"""
                                       {bitfield.Visibility} {field.Type} {getterName}() => 
                                          {field.Type.UpperFirstChar()}.FromBits(({field.CustomTypeFieldType})({field.Name}.ToBits() & ({field.CustomTypeFieldType})({bitfield.Type.ToTypeString()}.MaxValue >> ({InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)} - {field.Bits}))));
                                       """);
                }
            }
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
            {
                source.AppendLine($"""
                                   {bitfield.Visibility} {InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()} {getterName}() => 
                                       {field.Name};
                                   """);
            }
            else if (field.Unsigned)
            {
                var internalType = InternalTypeUtil.GetInternalType(field.Type);
                source.AppendLine($$"""
                                    {{bitfield.Visibility}} {{internalType?.ToTypeString()}} {{getterName}}() {
                                        var mask = {{bitfield.Type.ToTypeString()}}.MaxValue >> ({{InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)}} - {{field.Bits}});
                                        return ({{InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()}})({{field.Name}} & ({{InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()}})mask);
                                    }
                                    """);
            }
            else
            {
                var fieldTypeBits =
                    InternalTypeUtil.GetBitsFromInternalType(
                        (InternalTypeUtil.InternalType)InternalTypeUtil.GetInternalType(field.Type)!);
                source.AppendLine($$"""
                                    {{bitfield.Visibility}} {{InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()}} {{getterName}}() {
                                        var shift = {{fieldTypeBits}} - {{field.Bits}};
                                        return ({{InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()}})(({{InternalTypeUtil.GetInternalType(field.Type)?.ToTypeString()}})({{field.Name}} << shift) >> shift);
                                    }
                                    """);
            }
        }

        return string.Join("\n", source);
    }

    /// <summary>
    ///     Generates the source code for the field setters.
    /// </summary>
    public static string GenerateFieldSettersSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();

        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveSetter))
        {
            var setterName = $"Set{field.Name.ToPascalCase()}";
            var fieldBitsConst = $"{field.Name.ToSnakeCase().ToUpper()}_BITS";

            if (field.CustomType)
            {
                source.AppendLine($"""
                                   {bitfield.Visibility} void {setterName}({field.Type} val) => 
                                       {field.Name} = val;
                                   """);
            }
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
            {
                source.AppendLine($"""
                                   {bitfield.Visibility} void {setterName}({field.Type} val) => 
                                       {field.Name} = val;
                                   """);
            }
            else
            {
                var internalType = InternalTypeUtil.GetInternalType(field.Type);
                source.AppendLine($$"""
                                    {{bitfield.Visibility}} void {{setterName}}({{field.Type}} val) {
                                        var mask = {{bitfield.Type.ToTypeString()}}.MaxValue >> ({{InternalTypeUtil.GetBitsFromInternalType(bitfield.Type)}} - {{field.Bits}});
                                        {{field.Name}} = ({{field.Type}})(val & ({{field.Type}})mask);
                                    }
                                    """);
            }
        }

        return string.Join("\n", source);
    }
}