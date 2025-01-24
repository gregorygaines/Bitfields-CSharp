namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt, BitOrder.Msb)]
public partial class MsbBitfield
{
    private byte a = 0x12;
    private byte b = 0x34;
    private byte c = 0x56;
    private byte d = 0x78;
}

public class MostSignificantBitTests
{
    [Test]
    public void Msb()
    {
        var msb = new MsbBitfield();

        Assert.Multiple(() =>
        {
            Assert.That(msb.GetA(), Is.EqualTo(0x12));
            Assert.That(msb.GetB(), Is.EqualTo(0x34));
            Assert.That(msb.GetC(), Is.EqualTo(0x56));
            Assert.That(msb.GetD(), Is.EqualTo(0x78));
            Assert.That(msb.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(MsbBitfield.A_OFFSET, Is.EqualTo(24));
            Assert.That(MsbBitfield.B_OFFSET, Is.EqualTo(16));
            Assert.That(MsbBitfield.C_OFFSET, Is.EqualTo(8));
            Assert.That(MsbBitfield.D_OFFSET, Is.EqualTo(0));
        });
    }

    [Test]
    public void FromBits()
    {
        var msb = MsbBitfield.FromBits(0x12345678);

        Assert.Multiple(() =>
        {
            Assert.That(msb.GetA(), Is.EqualTo(0x12));
            Assert.That(msb.GetB(), Is.EqualTo(0x34));
            Assert.That(msb.GetC(), Is.EqualTo(0x56));
            Assert.That(msb.GetD(), Is.EqualTo(0x78));
            Assert.That(msb.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(MsbBitfield.A_OFFSET, Is.EqualTo(24));
            Assert.That(MsbBitfield.B_OFFSET, Is.EqualTo(16));
            Assert.That(MsbBitfield.C_OFFSET, Is.EqualTo(8));
            Assert.That(MsbBitfield.D_OFFSET, Is.EqualTo(0));
        });
    }
}