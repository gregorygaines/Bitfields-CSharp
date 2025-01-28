//HintName: BitfieldIgnordMethos.g.cs
namespace Bitfields.CSharp.Tests.Snapshot.Cases
{
    public partial class BitfieldIgnordMethos {
   public const int D_BITS = 8;
public const int D_OFFSET = 24;
public const int C_BITS = 8;
public const int C_OFFSET = 16;
public const int B_BITS = 8;
public const int B_OFFSET = 8;
public const int A_BITS = 8;
public const int A_OFFSET = 0;

   public BitfieldIgnordMethos () {
this.d = 0;
this.c = 0;
this.b = 0;
this.a = 0;

}
public BitfieldIgnordMethos (bool withoutDefaults) {
if (withoutDefaults) {
this.d = (byte)0;
this.c = (byte)0;
this.b = (byte)0;
this.a = (byte)0;

}
else {
this.d = 0;
this.c = 0;
this.b = 0;
this.a = 0;

}
}

   public byte GetD() {
    var mask = uint.MaxValue >> (32 - 8);
    return (byte)(d & (byte)mask);
}
public byte GetC() {
    var mask = uint.MaxValue >> (32 - 8);
    return (byte)(c & (byte)mask);
}
public byte GetB() {
    var mask = uint.MaxValue >> (32 - 8);
    return (byte)(b & (byte)mask);
}
public byte GetA() {
    var mask = uint.MaxValue >> (32 - 8);
    return (byte)(a & (byte)mask);
}

   public void SetD(byte val) {
    var mask = uint.MaxValue >> (32 - 8);
    d = (byte)(val & (byte)mask);
}
public void SetC(byte val) {
    var mask = uint.MaxValue >> (32 - 8);
    c = (byte)(val & (byte)mask);
}
public void SetB(byte val) {
    var mask = uint.MaxValue >> (32 - 8);
    b = (byte)(val & (byte)mask);
}
public void SetA(byte val) {
    var mask = uint.MaxValue >> (32 - 8);
    a = (byte)(val & (byte)mask);
}

   public static BitfieldIgnordMethos FromBits(uint bits)
{
    var bitfield = new BitfieldIgnordMethos();
    bitfield.SetD((byte)(bits >> D_OFFSET & (uint.MaxValue >> (32 - 8))));
bitfield.SetC((byte)(bits >> C_OFFSET & (uint.MaxValue >> (32 - 8))));
bitfield.SetB((byte)(bits >> B_OFFSET & (uint.MaxValue >> (32 - 8))));
bitfield.SetA((byte)(bits >> A_OFFSET & (uint.MaxValue >> (32 - 8))));

    return bitfield;
}

   public static BitfieldIgnordMethos FromBitsWithDefaults(uint bits)
{
    var bitfield = BitfieldIgnordMethos.FromBits(bits);
    bitfield.d = 0;
bitfield.c = 0;
bitfield.b = 0;
bitfield.a = 0;

    return bitfield;
}

   public uint ToBits() {
uint bits = (uint)0;
bits |= (uint)((uint)GetD() << D_OFFSET);
bits |= (uint)((uint)GetC() << C_OFFSET);
bits |= (uint)((uint)GetB() << B_OFFSET);
bits |= (uint)((uint)GetA() << A_OFFSET);
return bits;
}

   private void SetFieldDefaults() {
this.d = 0;
this.c = 0;
this.b = 0;
this.a = 0;
}

   public class BitfieldIgnordMethosBuilder {
private BitfieldIgnordMethos _bitfield;
public BitfieldIgnordMethosBuilder(bool withoutDefaults) {
_bitfield = new BitfieldIgnordMethos(withoutDefaults);
}
public BitfieldIgnordMethosBuilder() {
_bitfield = new BitfieldIgnordMethos();
}
public BitfieldIgnordMethosBuilder(uint val) {
_bitfield = BitfieldIgnordMethos.FromBits(val);
}
public BitfieldIgnordMethosBuilder WithD(byte d) {
_bitfield.SetD(d);
return this;
}
public BitfieldIgnordMethosBuilder WithC(byte c) {
_bitfield.SetC(c);
return this;
}
public BitfieldIgnordMethosBuilder WithB(byte b) {
_bitfield.SetB(b);
return this;
}
public BitfieldIgnordMethosBuilder WithA(byte a) {
_bitfield.SetA(a);
return this;
}

public BitfieldIgnordMethos Build() {
return _bitfield;
}
}

   public BitfieldIgnordMethosBuilder ToBuilder() {
    return new BitfieldIgnordMethosBuilder(this.ToBits());
}

}
}