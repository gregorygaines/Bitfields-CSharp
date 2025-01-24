//HintName: CustomType.g.cs
namespace Bitfields.CSharp.Tests.Snapshot.Cases
{
     partial class CustomType {
    const int FIELD_BITS = 8;
 const int FIELD_OFFSET = 0;

    CustomType () {
this._field = (byte)0;

}
 CustomType (bool withoutDefaults) {
if (withoutDefaults) {
this._field = (byte)0;

}
else {
this._field = (byte)0;

}
}

    byte GetField() {
    var mask = byte.MaxValue >> (8 - 8);
    return (byte)(_field & (byte)mask);
}

    void SetField(byte val) {
    var mask = byte.MaxValue >> (8 - 8);
    _field = (byte)(val & (byte)mask);
}

    static CustomType FromBits(byte bits)
{
    var bitfield = new CustomType();
    bitfield.SetField((byte)(bits >> FIELD_OFFSET & (byte.MaxValue >> (8 - 8))));

    return bitfield;
}

    static CustomType FromBitsWithDefaults(byte bits)
{
    var bitfield = CustomType.FromBits(bits);
    
    return bitfield;
}

    byte ToBits() {
byte bits = (byte)0;
bits |= (byte)((byte)GetField() << FIELD_OFFSET);
return bits;
}

   private void SetFieldDefaults() {
}

    class CustomTypeBuilder {
private CustomType _bitfield;
 CustomTypeBuilder(bool withoutDefaults) {
_bitfield = new CustomType(withoutDefaults);
}
 CustomTypeBuilder() {
_bitfield = new CustomType();
}
 CustomTypeBuilder(byte val) {
_bitfield = CustomType.FromBits(val);
}
 CustomTypeBuilder WithField(byte _field) {
_bitfield.SetField(_field);
return this;
}

 CustomType Build() {
return _bitfield;
}
}

    CustomTypeBuilder ToBuilder() {
    return new CustomTypeBuilder(this.ToBits());
}

}
}