#pragma warning disable CS9113 // These fields only exist to provide metadata for the BitfieldGenerator.

namespace Bitfields.CSharp;

[AttributeUsage(AttributeTargets.Field)]
public class BitsAttribute(byte bits = 0, CustomFieldType customFieldType = CustomFieldType.Unknown)
    : Attribute;