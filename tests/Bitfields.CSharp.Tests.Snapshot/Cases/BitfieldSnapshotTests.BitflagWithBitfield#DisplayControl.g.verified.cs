//HintName: DisplayControl.g.cs
namespace Bitfields.CSharp.Tests.Snapshot.Cases
{
    public partial class DisplayControl {
   public const int AUDIO_MODE_BITS = 4;
public const int AUDIO_MODE_OFFSET = 4;
public const int RENDER_MODE_BITS = 4;
public const int RENDER_MODE_OFFSET = 0;

   public DisplayControl () {
this.audioMode = (byte)0;
this.renderMode = RenderModeExtensions.FromBits(0);

}
public DisplayControl (bool withoutDefaults) {
if (withoutDefaults) {
this.audioMode = (byte)0;
this.renderMode = RenderModeExtensions.FromBits(0);

}
else {
this.audioMode = (byte)0;
this.renderMode = RenderModeExtensions.FromBits(0);

}
}

   public byte GetAudioMode() {
    var mask = byte.MaxValue >> (8 - 4);
    return (byte)(audioMode & (byte)mask);
}
public RenderMode GetRenderMode() => RenderModeExtensions.FromBits((byte)(renderMode.ToBits() & (byte)(byte.MaxValue >> (8 - 4))));

   public void SetAudioMode(byte val) {
    var mask = byte.MaxValue >> (8 - 4);
    audioMode = (byte)(val & (byte)mask);
}
public void SetRenderMode(RenderMode val) => 
    renderMode = val;

   public static DisplayControl FromBits(byte bits)
{
    var bitfield = new DisplayControl();
    bitfield.SetAudioMode((byte)(bits >> AUDIO_MODE_OFFSET & (byte.MaxValue >> (8 - 4))));
bitfield.SetRenderMode(RenderModeExtensions.FromBits((byte)(bits >> RENDER_MODE_OFFSET & (byte)(byte.MaxValue >> (8 - 4)))));

    return bitfield;
}

   public static DisplayControl FromBitsWithDefaults(byte bits)
{
    var bitfield = DisplayControl.FromBits(bits);
    
    return bitfield;
}

   public byte ToBits() {
byte bits = (byte)0;
bits |= (byte)((byte)GetAudioMode() << AUDIO_MODE_OFFSET);
bits |= (byte)((byte)GetRenderMode().ToBits() << RENDER_MODE_OFFSET);
return bits;
}

   private void SetFieldDefaults() {
}

   public class DisplayControlBuilder {
private DisplayControl _bitfield;
public DisplayControlBuilder(bool withoutDefaults) {
_bitfield = new DisplayControl(withoutDefaults);
}
public DisplayControlBuilder() {
_bitfield = new DisplayControl();
}
public DisplayControlBuilder(byte val) {
_bitfield = DisplayControl.FromBits(val);
}
public DisplayControlBuilder WithAudioMode(byte audioMode) {
_bitfield.SetAudioMode(audioMode);
return this;
}
public DisplayControlBuilder WithRenderMode(RenderMode renderMode) {
_bitfield.SetRenderMode(renderMode);
return this;
}

public DisplayControl Build() {
return _bitfield;
}
}

   public DisplayControlBuilder ToBuilder() {
    return new DisplayControlBuilder(this.ToBits());
}

}
}