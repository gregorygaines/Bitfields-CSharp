namespace Bitfields.CSharp.Tests.Snapshot;

[TestFixture]
public class BitfieldSnapshotTests
{
    [Test]
    public async Task BitfieldWithoutPartial()
    {
        var source = await CaseLoader.LoadCase("BitfieldWithoutPartial");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task UnsupportedFieldType()
    {
        var source = await CaseLoader.LoadCase("UnsupportedFieldType");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task CustomTypeWithoutBits()
    {
        var source = await CaseLoader.LoadCase("CustomTypeWithoutBits");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task CustomTypeWithoutFieldType()
    {
        var source = await CaseLoader.LoadCase("CustomTypeWithoutFieldType");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task FieldTypeTooSmallToContainBits()
    {
        var source = await CaseLoader.LoadCase("FieldTypeTooSmallToContainBits");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task FieldBitsZero()
    {
        var source = await CaseLoader.LoadCase("FieldBitsZero");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task TotalFieldBitsLessThanBitfield()
    {
        var source = await CaseLoader.LoadCase("TotalFieldBitsLessThanBitfield");

        await SnapshotVerifier.Verify(source);
    }

    [Test]
    public async Task TotalFieldBitsGreaterThanBitfield()
    {
        var source = await CaseLoader.LoadCase("TotalFieldBitsGreaterThanBitfield");

        await SnapshotVerifier.Verify(source);
    }
    
    [Test]
    public async Task BitfieldWithIgnoredMethods()
    {
        var source = await CaseLoader.LoadCase("BitfieldWithIgnoredMethods");

        await SnapshotVerifier.Verify(source);
    }
}