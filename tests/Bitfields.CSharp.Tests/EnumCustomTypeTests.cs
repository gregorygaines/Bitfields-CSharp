namespace Bitfields.CSharp.Tests;

public enum CustomEnumType
{
    BfBase = int.MaxValue,
    A = 0,
    B = 1,
    C = 2,
    D = 3
}

public static class CustomEnumTypeExtensions
{
    public static CustomEnumType FromBits(this CustomEnumType _, byte val)
    {
        return val switch
        {
            0 => CustomEnumType.A,
            1 => CustomEnumType.B,
            2 => CustomEnumType.C,
            3 => CustomEnumType.D,
            _ => CustomEnumType.A
        };
    }

    public static byte ToBits(this CustomEnumType val)
    {
        return (byte)val;
    }
}

[Bitfield(BitfieldType.UInt)]
public partial class BitfieldWithEnumField
{
    private byte a;
    private byte b;
    private byte c;
    [Bits(8, CustomFieldType.Byte, CustomFieldBase.Enum)]
    private CustomEnumType d;
}

[TestFixture]
public class EnumCustomTypeTests
{
    [Test]
    public void EnumField()
    {
        _ = new BitfieldWithEnumField();

        Assert.Pass();
    }

    [Test]
    public void EnumFieldFromBits()
    {
        var bitfieldWithEnumField = BitfieldWithEnumField.FromBits(0x12345678);

        Assert.Multiple(() =>
        {
            Assert.That(bitfieldWithEnumField.GetD(), Is.EqualTo(CustomEnumType.A));
            Assert.That(bitfieldWithEnumField.ToBits(), Is.EqualTo(0x345678));
        });
    }

    [Test]
    public void EnumFieldSetField()
    {
        var bitfield = new BitfieldWithEnumField();

        bitfield.SetD(CustomEnumType.C);

        Assert.That(bitfield.GetD(), Is.EqualTo(CustomEnumType.C));
    }
}