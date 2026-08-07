namespace Bitfields.CSharp.Bitflag;

/// <summary>
/// Marks an enum as a bitflag.
/// </summary>
[AttributeUsage(AttributeTargets.Enum)]
public class BitflagAttribute(BitfieldType type) : Attribute;
