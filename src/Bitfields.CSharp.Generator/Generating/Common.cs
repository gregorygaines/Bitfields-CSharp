using System.Text;
using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class Common
{
    public const string CustomFieldEnumBaseEnumName = "BfBase";

    private const string SetFieldDefaultsMethodName = "SetFieldDefaults";

    public static string GenerateSettingDefaultsSource(List<ParsedBitfieldField> parsedFields)
    {
        var source = new StringBuilder();
        source.AppendLine($$"""private void {{SetFieldDefaultsMethodName}}() {""");
        foreach (var field in parsedFields.Where(f => f.Default != null))
            if (field.CustomType)
                source.AppendLine($"this.{field.Name} = {field.Default};");
            else if (InternalTypeUtil.GetInternalType(field.Type) == InternalTypeUtil.InternalType.Bool)
                source.AppendLine($"this.{field.Name} = {field.Default};");
            else
                source.AppendLine($"this.{field.Name} = {field.Default};");

        source.AppendLine("}");

        return string.Join("\n", source);
    }

    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        var words = str.Split(["_", " "], StringSplitOptions.RemoveEmptyEntries);

        return words.Aggregate(string.Empty, (current, word) => current + (char.ToUpperInvariant(word[0]) + word.Substring(1)));
    }

    public static string ToSnakeCase(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;

        var sb = new StringBuilder();
        if (str[0] != '_') sb.Append(char.ToLowerInvariant(str[0]));

        for (var i = 1; i < str.Length; i++)
        {
            var c = str[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    public static string UpperFirstChar(this string input)
    {
        return input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };
    }
}