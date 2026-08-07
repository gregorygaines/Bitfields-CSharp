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

[Bitfield(BitfieldType.UInt128)]
public partial class UInt128Bitfield
{
    private ulong low;
    private ulong high;
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
        _ = new UInt128Bitfield();
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
        var uInt128Bitfield = new UInt128Bitfield();

        byteBitfield.SetByte(0x12);
        charBitfield.SetChar('c');
        uShortBitfield.SetUshort(0x1234);
        uIntBitfield.SetUint(0x12345678);
        uLongBitfield.SetUlong(0x123456789abcdef0);
        uInt128Bitfield.SetLow(0x0123456789abcdef);
        uInt128Bitfield.SetHigh(0xfedcba9876543210);
        var expectedUInt128 = ((UInt128)0xfedcba9876543210UL << 64) | 0x0123456789abcdefUL;
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
            Assert.That(uInt128Bitfield.GetLow(), Is.EqualTo(0x0123456789abcdef));
            Assert.That(uInt128Bitfield.GetHigh(), Is.EqualTo(0xfedcba9876543210));
            Assert.That(uInt128Bitfield.ToBits(), Is.EqualTo(expectedUInt128));
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
        var expectedUInt128 = ((UInt128)0xfedcba9876543210UL << 64) | 0x0123456789abcdefUL;
        var uInt128Bitfield = UInt128Bitfield.FromBits(expectedUInt128);
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
            Assert.That(uInt128Bitfield.GetLow(), Is.EqualTo(0x0123456789abcdef));
            Assert.That(uInt128Bitfield.GetHigh(), Is.EqualTo(0xfedcba9876543210));
            Assert.That(uInt128Bitfield.ToBits(), Is.EqualTo(expectedUInt128));
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
        var uInt128Bitfield = new UInt128Bitfield.UInt128BitfieldBuilder()
            .WithLow(0x0123456789abcdef)
            .WithHigh(0xfedcba9876543210)
            .Build();
        var expectedUInt128 = ((UInt128)0xfedcba9876543210UL << 64) | 0x0123456789abcdefUL;

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
            Assert.That(uInt128Bitfield.GetLow(), Is.EqualTo(0x0123456789abcdef));
            Assert.That(uInt128Bitfield.GetHigh(), Is.EqualTo(0xfedcba9876543210));
            Assert.That(uInt128Bitfield.ToBits(), Is.EqualTo(expectedUInt128));
        });
    }
}
