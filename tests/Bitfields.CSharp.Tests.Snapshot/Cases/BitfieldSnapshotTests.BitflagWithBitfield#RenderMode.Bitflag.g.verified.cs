//HintName: RenderMode.Bitflag.g.cs
namespace Bitfields.CSharp.Tests.Snapshot.Cases {
public static class RenderModeExtensions {
    public static global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode FromBits(byte bits) => bits switch {
        0 => global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode.Normal,
        1 => global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode.Mirror,
        2 => global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode.Flip,
        3 => global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode.Hidden,
        _ => global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode.Normal
    };
    public static global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode FromBits(this global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode _, byte bits) => FromBits(bits);
    public static byte ToBits(this global::Bitfields.CSharp.Tests.Snapshot.Cases.RenderMode value) => (byte)value;
}

}