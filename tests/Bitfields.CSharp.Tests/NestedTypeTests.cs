namespace Bitfields.CSharp.Tests;

[Bitfield(BitfieldType.Byte)]
public partial class BaseBitfield
{
    [Bits(8, CustomFieldType.Byte)] public NestedBitfield nested = new();
}

[Bitfield(BitfieldType.Byte)]
public partial class NestedBitfield
{
    private byte a = 33;
}

[TestFixture]
public class NestedTypeTests
{
    [Test]
    public void NestedType()
    {
        var bitfield = new BaseBitfield();

        Assert.That(bitfield.GetNested().ToBits(), Is.EqualTo(33));
    }
}