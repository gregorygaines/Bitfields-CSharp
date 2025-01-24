using Bitfields.CSharp.Generator.Parsing;

namespace Bitfields.CSharp.Generator.Generating;

public static class FieldUtil
{
    public static bool DoesFieldHaveGetter(ParsedBitfieldField field)
    {
        return field is { Padding: false };
    }

    public static bool DoesFieldHaveSetter(ParsedBitfieldField field)
    {
        return field is { Padding: false };
    }
}