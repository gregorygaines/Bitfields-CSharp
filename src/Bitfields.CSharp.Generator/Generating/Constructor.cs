using System.Text;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class Constructor
{
    /// <summary>
    ///     Generates the source code for the bitfield constructor.
    /// </summary>
    public static string GeneratorConstructorSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        source.AppendLine($"{bitfield.Visibility} {bitfield.Name} () {{");
        source.AppendLine($"{GenerateSettingFieldsToDefaultsImplSource(fields)}");
        source.AppendLine("}");
        source.AppendLine($"{bitfield.Visibility} {bitfield.Name} (bool withoutDefaults) {{");
        source.AppendLine("if (withoutDefaults) {");
        source.AppendLine($"{GenerateSettingFieldsToZeroImplSource(fields)}");
        source.AppendLine("}");
        source.AppendLine("else {");
        source.AppendLine($"{GenerateSettingFieldsToDefaultsImplSource(fields)}");
        source.AppendLine("}");
        source.AppendLine("}");
        return string.Join("\n", source);
    }

    private static string GenerateSettingFieldsToDefaultsImplSource(List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveGetter))
            if (field.CustomType)
            {
                if (field.Default != null)
                    source.AppendLine($"this.{field.Name} = {field.Default};");
                else
                    source.AppendLine($"this.{field.Name} = {field.Type}.FromBits(0);");
            }
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
            {
                if (field.Default != null)
                    source.AppendLine($"this.{field.Name} = {field.Default};");
                else
                    source.AppendLine($"this.{field.Name} = false;");
            }
            else
            {
                if (field.Default != null)
                    source.AppendLine($"this.{field.Name} = {field.Default};");
                else
                    source.AppendLine($"this.{field.Name} = ({field.Type})0;");
            }

        return string.Join("\n", source);
    }

    private static string GenerateSettingFieldsToZeroImplSource(List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveGetter))
            if (field.CustomType)
                source.AppendLine($"this.{field.Name} = {field.Type}.FromBits(0);");
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
                source.AppendLine($"this.{field.Name} = false;");
            else
                source.AppendLine($"this.{field.Name} = ({field.Type})0;");

        return string.Join("\n", source);
    }
}