namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.Byte)]
public partial class ByteBitfield
{
    private byte _byte;
}

[Bitfield(BitfieldType.Char)]
public partial class CharBitfield
{
    private char _char;
}

[Bitfield(BitfieldType.UShort)]
public partial class UShortBitfield
{
    private ushort _ushort;
}

[Bitfield(BitfieldType.UInt)]
public partial class UIntBitfield
{
    private uint _uint;
}

[Bitfield(BitfieldType.ULong)]
public partial class ULongBitfield
{
    private ulong _ulong;
}

[TestFixture]
public class PrimitiveBitfieldsTests
{
    [Test]
    public void PrimitiveBitfields_Compiles()
    {
        _ = new ByteBitfield();
        _ = new CharBitfield();
        _ = new UShortBitfield();
        _ = new UIntBitfield();
        _ = new ULongBitfield();
        Assert.Multiple(() =>
        {
            Assert.That(ByteBitfield.BYTE_BITS, Is.EqualTo(8));
            Assert.That(ByteBitfield.BYTE_OFFSET, Is.EqualTo(0));
        });
    }

    [Test]
    public void SettingFields()
    {
        var byteBitfield = new ByteBitfield();
        var charBitfield = new CharBitfield();
        var uShortBitfield = new UShortBitfield();
        var uIntBitfield = new UIntBitfield();
        var uLongBitfield = new ULongBitfield();

        byteBitfield.SetByte(0x12);
        charBitfield.SetChar('c');
        uShortBitfield.SetUshort(0x1234);
        uIntBitfield.SetUint(0x12345678);
        uLongBitfield.SetUlong(0x123456789abcdef0);
        Assert.Multiple(() =>
        {
            Assert.That(byteBitfield.GetByte(), Is.EqualTo(0x12));
            Assert.That(byteBitfield.ToBits(), Is.EqualTo(0x12));
            Assert.That(charBitfield.GetChar(), Is.EqualTo('c'));
            Assert.That(charBitfield.ToBits(), Is.EqualTo('c'));
            Assert.That(uShortBitfield.GetUshort(), Is.EqualTo(0x1234));
            Assert.That(uShortBitfield.ToBits(), Is.EqualTo(0x1234));
            Assert.That(uIntBitfield.GetUint(), Is.EqualTo(0x12345678));
            Assert.That(uIntBitfield.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(uLongBitfield.GetUlong(), Is.EqualTo(0x123456789abcdef0));
            Assert.That(uLongBitfield.ToBits(), Is.EqualTo(0x123456789abcdef0));
        });
    }

    [Test]
    public void FromBits()
    {
        var byteBitfield = ByteBitfield.FromBits(0x12);
        var charBitfield = CharBitfield.FromBits('c');
        var uShortBitfield = UShortBitfield.FromBits(0x1234);
        var uIntBitfield = UIntBitfield.FromBits(0x12345678);
        var uLongBitfield = ULongBitfield.FromBits(0x123456789abcdef0);
        Assert.Multiple(() =>
        {
            Assert.That(byteBitfield.GetByte(), Is.EqualTo(0x12));
            Assert.That(byteBitfield.ToBits(), Is.EqualTo(0x12));
            Assert.That(charBitfield.GetChar(), Is.EqualTo('c'));
            Assert.That(charBitfield.ToBits(), Is.EqualTo('c'));
            Assert.That(uShortBitfield.GetUshort(), Is.EqualTo(0x1234));
            Assert.That(uShortBitfield.ToBits(), Is.EqualTo(0x1234));
            Assert.That(uIntBitfield.GetUint(), Is.EqualTo(0x12345678));
            Assert.That(uIntBitfield.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(uLongBitfield.GetUlong(), Is.EqualTo(0x123456789abcdef0));
            Assert.That(uLongBitfield.ToBits(), Is.EqualTo(0x123456789abcdef0));
        });
    }

    [Test]
    public void Builder()
    {
        var byteBitfield = new ByteBitfield.ByteBitfieldBuilder().WithByte(0x12).Build();
        var charBitfield = new CharBitfield.CharBitfieldBuilder().WithChar('c').Build();
        var uShortBitfield = new UShortBitfield.UShortBitfieldBuilder().WithUshort(0x1234).Build();
        var uIntBitfield = new UIntBitfield.UIntBitfieldBuilder().WithUint(0x12345678).Build();
        var uLongBitfield = new ULongBitfield.ULongBitfieldBuilder().WithUlong(0x123456789abcdef0).Build();

        Assert.Multiple(() =>
        {
            Assert.That(byteBitfield.GetByte(), Is.EqualTo(0x12));
            Assert.That(byteBitfield.ToBits(), Is.EqualTo(0x12));
            Assert.That(charBitfield.GetChar(), Is.EqualTo('c'));
            Assert.That(charBitfield.ToBits(), Is.EqualTo('c'));
            Assert.That(uShortBitfield.GetUshort(), Is.EqualTo(0x1234));
            Assert.That(uShortBitfield.ToBits(), Is.EqualTo(0x1234));
            Assert.That(uIntBitfield.GetUint(), Is.EqualTo(0x12345678));
            Assert.That(uIntBitfield.ToBits(), Is.EqualTo(0x12345678));
            Assert.That(uLongBitfield.GetUlong(), Is.EqualTo(0x123456789abcdef0));
            Assert.That(uLongBitfield.ToBits(), Is.EqualTo(0x123456789abcdef0));
        });
    }
}