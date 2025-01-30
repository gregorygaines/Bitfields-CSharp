using System.Text;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class Builder
{
    /// <summary>
    ///     Generates the source code for the bitfield builder class.
    /// </summary>
    public static string BuilderSource(ParsedBitfield bitfield, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();
        var builderName = $"{bitfield.Name}Builder".ToPascalCase();

        source.AppendLine($"{bitfield.Visibility} class {builderName} {{");
        source.AppendLine($"private {bitfield.Name} _bitfield;");
        source.AppendLine($"{bitfield.Visibility} {builderName}(bool withoutDefaults) {{");
        source.AppendLine($"_bitfield = new {bitfield.Name}(withoutDefaults);");
        source.AppendLine("}");
        source.AppendLine($"{bitfield.Visibility} {builderName}() {{");
        source.AppendLine($"_bitfield = new {bitfield.Name}();");
        source.AppendLine("}");
        source.AppendLine($"{bitfield.Visibility} {builderName}({bitfield.Type.ToTypeString()} val) {{");
        source.AppendLine($"_bitfield = {bitfield.Name}.FromBits(val);");
        source.AppendLine("}");
        source.AppendLine(GenerateFluentFieldsSetters(bitfield, builderName, fields));
        source.AppendLine($"{bitfield.Visibility} {bitfield.Name} Build() {{");
        source.AppendLine("return _bitfield;");
        source.AppendLine("}");
        source.AppendLine("}");

        return string.Join("\n", source);
    }

    private static string GenerateFluentFieldsSetters(ParsedBitfield bitfield, string builderName, List<ParsedBitfieldField> fields)
    {
        var source = new StringBuilder();

        foreach (var field in fields.Where(FieldUtil.DoesFieldHaveSetter))
        {
            var fluentName = $"With{field.Name.ToPascalCase()}";
            var setterName = $"Set{field.Name.ToPascalCase()}";
            source.AppendLine($"{bitfield.Visibility} {builderName} {fluentName}({field.Type} {field.Name}) {{");
            source.AppendLine($"_bitfield.{setterName}({field.Name});");
            source.AppendLine("return this;");
            source.AppendLine("}");
        }

        return string.Join("\n", source);
    }

    public static string ToBuilderSource(ParsedBitfield bitfield)
    {
        var source = new StringBuilder();
        var builderName = $"{bitfield.Name}Builder".ToPascalCase();

        source.AppendLine($$"""
                           {{bitfield.Visibility}} {{builderName}} ToBuilder() {
                               return new {{builderName}}(this.ToBits());
                           }
                           """);

        return string.Join("\n", source);
    }
}