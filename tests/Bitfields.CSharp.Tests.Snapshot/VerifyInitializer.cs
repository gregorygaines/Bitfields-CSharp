using System.Runtime.CompilerServices;

namespace Bitfields.CSharp.Tests.Snapshot;

public static class VerifyInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}