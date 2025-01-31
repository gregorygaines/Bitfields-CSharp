#pragma warning disable CS9113 // These fields only exist to provide metadata for the BitfieldGenerator.

namespace Bitfields.CSharp;

/// <summary>
/// Attribute to specify properties of a bitfield field.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class BitsAttribute(
    byte bits = 0,
    CustomFieldType customFieldType = CustomFieldType.Unknown,
    CustomFieldBase customFieldBase = CustomFieldBase.ClassOrStruct)
    : Attribute;