using Bitfields.CSharp.Bitflag;

namespace Bitfields.CSharp.Tests.Snapshot.Cases;

[Bitflag(BitfieldType.Byte)]
public enum RenderMode
{
    [Base] Normal = 0,
    Mirror = 1,
    Flip = 2,
    Hidden = 3
}

[Bitfield(BitfieldType.Byte)]
public partial class DisplayControl
{
    [Bits(4)]
    private RenderMode renderMode;

    [Bits(4)]
    private byte audioMode;
}
