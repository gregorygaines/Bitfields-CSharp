#pragma warning disable CS9113 // These fields only exist to provide metadata for the BitfieldGenerator.

namespace Bitfields.CSharp;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class BitfieldAttribute(
    BitfieldType type,
    BitOrder bitOrder = BitOrder.Lsb) : Attribute;