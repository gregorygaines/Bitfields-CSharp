namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.Byte)]
public partial class CustomBitfield
{
    [Bits(8, CustomFieldType.Byte)] public CustomType customType;
}

public class CustomType
{
    public static CustomType FromBits(byte val)
    {
        return new CustomType();
    }

    public byte ToBits()
    {
        return 0xFF;
    }
}

[TestFixture]
public class CustomTypeTests
{
    [Test]
    public void CustomType()
    {
        var bitfield = new CustomBitfield();

        Assert.That(bitfield.customType.ToBits(), Is.EqualTo(0xFF));
    }
}