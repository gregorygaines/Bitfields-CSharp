namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitfield(BitfieldType.UShort)]
public partial class Bitfield
{
    [Bits(8)]
    byte _field;
    
    [Bits(8)]
    byte _field1;
    
    [Bits(8)]
    byte _field2;
}