using NUnit.Framework;

namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt)]
public partial class MultipleByteFieldsBitfield
{
    private byte a = 0x12;
    private byte b = 0x34;
    private byte c = 0x56;
    private byte d = 0x78;
}

[Bitfield(BitfieldType.Byte)]
public partial class MultipleBoolFieldsBitfield
{
    private bool a = true;
    private bool b;
    private bool c = true;
    private bool d = true;
    private bool e;
    private bool f;
    private bool g;
    private bool h = true;
}

[Bitfield(BitfieldType.UInt)]
public partial class AllPossibleFieldsBitfield
{
    [Bits(1)] private byte a = 1;
    [Bits(1)] private sbyte b = -1;
    [Bits(1)] private char c = 'c';
    [Bits(1)] private ushort d = 0x1;
    [Bits(1)] private short e = -1;
    [Bits(1)] private uint f = 1;
    [Bits(1)] private int g = -1;
    [Bits(1)] private ulong h = 1;
    [Bits(1)] private long i = -1;
    [Bits(23)] private uint padding_;
}

[TestFixture]
public class MultipleFieldsTests
{
    [Test]
    public void WithDefaults()
    {
        var multipleByteFieldsBitfield = new MultipleByteFieldsBitfield();
        var multipleBoolFieldsBitfield = new MultipleBoolFieldsBitfield();
        var allPossibleFieldsBitfield = new AllPossibleFieldsBitfield();

        Assert.Multiple(() =>
        {
            Assert.That(multipleByteFieldsBitfield.GetA(), Is.EqualTo(0x12));
            Assert.That(multipleByteFieldsBitfield.GetB(), Is.EqualTo(0x34));
            Assert.That(multipleByteFieldsBitfield.GetC(), Is.EqualTo(0x56));
            Assert.That(multipleByteFieldsBitfield.GetD(), Is.EqualTo(0x78));
            Assert.That(multipleByteFieldsBitfield.ToBits(), Is.EqualTo(0x78563412));

            Assert.That(multipleBoolFieldsBitfield.GetA(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetB(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetC(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetD(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetE(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetF(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetG(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetH(), Is.True);

            Assert.That(allPossibleFieldsBitfield.GetA(), Is.EqualTo(1));
            Assert.That(allPossibleFieldsBitfield.GetB(), Is.EqualTo(-1));
            Assert.That(allPossibleFieldsBitfield.GetC(), Is.EqualTo('\u0001'));
            Assert.That(allPossibleFieldsBitfield.GetD(), Is.EqualTo(1));
            Assert.That(allPossibleFieldsBitfield.GetE(), Is.EqualTo(-1));
            Assert.That(allPossibleFieldsBitfield.GetF(), Is.EqualTo(1));
            Assert.That(allPossibleFieldsBitfield.GetG(), Is.EqualTo(-1));
            Assert.That(allPossibleFieldsBitfield.GetH(), Is.EqualTo(1));
            Assert.That(allPossibleFieldsBitfield.GetI(), Is.EqualTo(-1));
        });
    }

    [Test]
    public void WithoutDefaults()
    {
        var multipleByteFieldsBitfield = new MultipleByteFieldsBitfield(true);
        var multipleBoolFieldsBitfield = new MultipleBoolFieldsBitfield(true);
        var allPossibleFieldsBitfield = new AllPossibleFieldsBitfield(true);

        Assert.Multiple(() =>
        {
            Assert.That(multipleByteFieldsBitfield.GetA(), Is.EqualTo(0));
            Assert.That(multipleByteFieldsBitfield.GetB(), Is.EqualTo(0));
            Assert.That(multipleByteFieldsBitfield.GetC(), Is.EqualTo(0));
            Assert.That(multipleByteFieldsBitfield.GetD(), Is.EqualTo(0));

            Assert.That(multipleBoolFieldsBitfield.GetA(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetB(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetC(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetD(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetE(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetF(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetG(), Is.False);
            Assert.That(multipleBoolFieldsBitfield.GetH(), Is.False);

            Assert.That(allPossibleFieldsBitfield.GetA(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetB(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetC(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetD(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetE(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetF(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetG(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetH(), Is.EqualTo(0));
            Assert.That(allPossibleFieldsBitfield.GetI(), Is.EqualTo(0));
        });
    }

    [Test]
    public void Builder()
    {
        var multipleByteFieldsBitfield =
            new MultipleByteFieldsBitfield.MultipleByteFieldsBitfieldBuilder().WithA(0xFF).Build();
        var multipleBoolFieldsBitfield = new MultipleBoolFieldsBitfield.MultipleBoolFieldsBitfieldBuilder()
            .WithA(true)
            .WithB(true)
            .WithC(true)
            .WithD(true)
            .WithE(true)
            .WithF(true)
            .WithG(true)
            .WithH(true)
            .Build();

        Assert.Multiple(() =>
        {
            Assert.That(multipleByteFieldsBitfield.GetA(), Is.EqualTo(0xFF));
            Assert.That(multipleByteFieldsBitfield.GetB(), Is.EqualTo(0x34));
            Assert.That(multipleByteFieldsBitfield.GetC(), Is.EqualTo(0x56));
            Assert.That(multipleByteFieldsBitfield.GetD(), Is.EqualTo(0x78));

            Assert.That(multipleBoolFieldsBitfield.GetA(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetB(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetC(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetD(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetE(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetF(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetG(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetH(), Is.True);
        });
    }

    [Test]
    public void BuilderWithDefaults()
    {
        var multipleByteFieldsBitfield =
            new MultipleByteFieldsBitfield.MultipleByteFieldsBitfieldBuilder(true).WithA(0xFF).Build();
        var multipleBoolFieldsBitfield = new MultipleBoolFieldsBitfield.MultipleBoolFieldsBitfieldBuilder()
            .WithA(true)
            .WithB(true)
            .WithC(true)
            .WithD(true)
            .WithE(true)
            .WithF(true)
            .WithG(true)
            .WithH(true)
            .Build();
        Assert.Multiple(() =>
        {
            Assert.That(multipleByteFieldsBitfield.GetA(), Is.EqualTo(0xFF));
            Assert.That(multipleByteFieldsBitfield.GetB(), Is.EqualTo(0));
            Assert.That(multipleByteFieldsBitfield.GetC(), Is.EqualTo(0));
            Assert.That(multipleByteFieldsBitfield.GetD(), Is.EqualTo(0));

            Assert.That(multipleBoolFieldsBitfield.GetA(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetB(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetC(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetD(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetE(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetF(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetG(), Is.True);
            Assert.That(multipleBoolFieldsBitfield.GetH(), Is.True);
        });
    }
}