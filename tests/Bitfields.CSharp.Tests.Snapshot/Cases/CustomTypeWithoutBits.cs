namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitfield(BitfieldType.Byte)]
public partial class Bitfield
{
    CustomType _field;
}

[Bitfield(BitfieldType.Byte)]
partial class CustomType
{
    byte _field;
}