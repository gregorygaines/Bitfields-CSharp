namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitfield(BitfieldType.UShort)]
public partial class Bitfield
{
    [Bits(16)]
    byte _field;
}