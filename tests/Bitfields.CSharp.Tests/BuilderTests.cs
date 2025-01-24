namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt)]
public partial class BuilderBitfield
{
    private byte a = 0;
    private byte b = 0;
    private byte c = 0;
    private byte d = 0;
}

[TestFixture]
public class BuilderTests
{
    [Test]
    public void Builder()
    {
        var builder = new BuilderBitfield.BuilderBitfieldBuilder();
        
        var bitfield = builder.WithA(0x12).WithB(0x34).WithC(0x56).WithD(0x78).Build();
        
        Assert.That(bitfield.ToBits(), Is.EqualTo(0x78563412));
        Assert.That(bitfield.GetA(), Is.EqualTo(0x12));
        Assert.That(bitfield.GetB(), Is.EqualTo(0x34));
        Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
        Assert.That(bitfield.GetD(), Is.EqualTo(0x78));
    }
    
    [Test]
    public void ToBuilder()
    {
        var originalBitfield = new BuilderBitfield.BuilderBitfieldBuilder().WithA(0x12).WithB(0x34).WithC(0x56).WithD(0x78).Build();

        var bitfield = originalBitfield.ToBuilder().WithA(0xFF).Build();
        
        Assert.That(bitfield.ToBits(), Is.EqualTo(0x785634FF));
        Assert.That(bitfield.GetA(), Is.EqualTo(0xFF));
        Assert.That(bitfield.GetB(), Is.EqualTo(0x34));
        Assert.That(bitfield.GetC(), Is.EqualTo(0x56));
        Assert.That(bitfield.GetD(), Is.EqualTo(0x78));
        Assert.That(originalBitfield.ToBits(), Is.EqualTo(0x78563412));
        Assert.That(originalBitfield.GetA(), Is.EqualTo(0x12));
        Assert.That(originalBitfield.GetB(), Is.EqualTo(0x34));
        Assert.That(originalBitfield.GetC(), Is.EqualTo(0x56));
        Assert.That(originalBitfield.GetD(), Is.EqualTo(0x78));
    }
}