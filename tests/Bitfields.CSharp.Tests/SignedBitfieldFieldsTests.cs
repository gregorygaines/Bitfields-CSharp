namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.UInt)]
public partial class SignedFieldsBitfield
{
    [Bits(4)] private sbyte a = 0xF;

    [Bits(28)] private int padding_;
}

[TestFixture]
public class SignedBitfieldFieldsTests
{
    [Test]
    public void SignExtended()
    {
        var signedFieldsBitfield = new SignedFieldsBitfield();

        Assert.That(signedFieldsBitfield.GetA(), Is.EqualTo(-1));
    }

    [Test]
    public void NonSignExtended()
    {
        var signedFieldsBitfield = new SignedFieldsBitfield.SignedFieldsBitfieldBuilder().WithA(1).Build();

        Assert.That(signedFieldsBitfield.GetA(), Is.EqualTo(1));
    }
}