namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt, BitOrder.Msb)]
public partial class Bitfield
{
    private byte a = 0x12;
    private byte b = 0x34;
    private byte c = 0x56;
    private byte d = 0x78;
}

[TestFixture]
public class ConstructorTests
{
    [Test]
    public void Constructor()
    {
        var bitfield = new Bitfield();

        Assert.Multiple(() =>
        {
            Assert.That(bitfield.GetA(), Is.EqualTo(0x12));
            Assert.That(bitfield.GetB(), Is.EqualTo(0x34));
            Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
            Assert.That(bitfield.GetD(), Is.EqualTo(0x78));
            Assert.That(bitfield.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(Bitfield.A_OFFSET, Is.EqualTo(24));
            Assert.That(Bitfield.B_OFFSET, Is.EqualTo(16));
            Assert.That(Bitfield.C_OFFSET, Is.EqualTo(8));
            Assert.That(Bitfield.D_OFFSET, Is.EqualTo(0));
        });
    }

    [Test]
    public void ConstructorWithDefaults()
    {
        var bitfield = new Bitfield(false);

        Assert.Multiple(() =>
        {
            Assert.That(bitfield.GetA(), Is.EqualTo(0x12));
            Assert.That(bitfield.GetB(), Is.EqualTo(0x34));
            Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
            Assert.That(bitfield.GetD(), Is.EqualTo(0x78));
            Assert.That(bitfield.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(Bitfield.A_OFFSET, Is.EqualTo(24));
            Assert.That(Bitfield.B_OFFSET, Is.EqualTo(16));
            Assert.That(Bitfield.C_OFFSET, Is.EqualTo(8));
            Assert.That(Bitfield.D_OFFSET, Is.EqualTo(0));
        });
    }

    [Test]
    public void ConstructorWithoutDefaults()
    {
        var bitfield = new Bitfield(true);

        Assert.Multiple(() =>
        {
            Assert.That(bitfield.GetA(), Is.EqualTo(0));
            Assert.That(bitfield.GetB(), Is.EqualTo(0));
            Assert.That(bitfield.GetC(), Is.EqualTo(0));
            Assert.That(bitfield.GetD(), Is.EqualTo(0));
            Assert.That(bitfield.ToBits(), Is.EqualTo(0));
            Assert.That(Bitfield.A_OFFSET, Is.EqualTo(24));
            Assert.That(Bitfield.B_OFFSET, Is.EqualTo(16));
            Assert.That(Bitfield.C_OFFSET, Is.EqualTo(8));
            Assert.That(Bitfield.D_OFFSET, Is.EqualTo(0));
        });
    }
}