namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitfield(BitfieldType.Byte)]
public partial class Bitfield
{
    [Bits(8)]
    CustomType _field;
}

[Bitfield(BitfieldType.Byte)]
partial class CustomType
{
    byte _field;
}