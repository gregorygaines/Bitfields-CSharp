namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt)]
public partial class FromBitsBitfield
{
    private byte a = 0x12;
    private byte b;
    private byte c = 0x56;
    private byte d;
}

[TestFixture]
public class FromBitsTests
{
    [Test]
    public void FromBits()
    {
        var bitfield = FromBitsBitfield.FromBits(0x78563412);

        Assert.That(bitfield.ToBits(), Is.EqualTo(0x78563412));
        Assert.That(bitfield.GetA(), Is.EqualTo(0x12));
        Assert.That(bitfield.GetB(), Is.EqualTo(0x34));
        Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
        Assert.That(bitfield.GetD(), Is.EqualTo(0x78));
    }

    [Test]
    public void FromBitsWithDefaults()
    {
        var bitfield = FromBitsBitfield.FromBitsWithDefaults(0xFFFFFFFF);

        Assert.That(bitfield.ToBits(), Is.EqualTo(0xFF56FF12));
        Assert.That(bitfield.GetA(), Is.EqualTo(0x12));
        Assert.That(bitfield.GetB(), Is.EqualTo(0xFF));
        Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
        Assert.That(bitfield.GetD(), Is.EqualTo(0xFF));
    }
}