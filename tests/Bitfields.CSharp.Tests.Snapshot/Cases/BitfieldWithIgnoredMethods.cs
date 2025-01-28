namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitfield(BitfieldType.UInt)]
public partial class BitfieldIgnordMethos
{
    private byte a = 0;
    private byte b = 0;
    private byte c = 0;
    private byte d = 0;

    public void Hello() { }
}