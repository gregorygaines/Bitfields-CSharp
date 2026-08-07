using Bitfields.CSharp.Bitflag;

namespace Bitfields.CSharp.Tests;

[Bitflag(BitfieldType.Byte)]
public enum RenderMode
{
    [Base] Normal = 0,
    Mirror = 1,
    Flip = 2,
    Hidden = 3
}

[Bitflag(BitfieldType.Byte)]
public enum AudioMode
{
    [Base] Stereo = 0,
    Mono = 1,
    Mute = 2,
    Surround = 3
}

[Bitfield(BitfieldType.Byte)]
public partial class DisplayControl
{
    [Bits(4)]
    private RenderMode renderMode = RenderMode.Normal;

    [Bits(4)]
    private AudioMode audioMode = AudioMode.Stereo;
}

[TestFixture]
public class BitflagTests
{
    [Test]
    public void ConvertsEnumValues()
    {
        Assert.That(RenderModeExtensions.FromBits(2), Is.EqualTo(RenderMode.Flip));
        Assert.That(RenderMode.Mirror.ToBits(), Is.EqualTo(1));
        Assert.That(RenderModeExtensions.FromBits(15), Is.EqualTo(RenderMode.Normal));
        Assert.That(AudioModeExtensions.FromBits(15), Is.EqualTo(AudioMode.Stereo));
    }

    [Test]
    public void PacksBitflagFields()
    {
        var display = new DisplayControl.DisplayControlBuilder()
            .WithRenderMode(RenderMode.Mirror)
            .WithAudioMode(AudioMode.Mute)
            .Build();

        Assert.That(display.GetRenderMode(), Is.EqualTo(RenderMode.Mirror));
        Assert.That(display.GetAudioMode(), Is.EqualTo(AudioMode.Mute));
        Assert.That(display.ToBits(), Is.EqualTo(0x21));
    }
}
