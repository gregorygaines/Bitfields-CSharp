using Bitfields.CSharp.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Bitfields.CSharp.Tests.Snapshot;

public static class SnapshotVerifier
{
    private static readonly IEnumerable<string> SDefaultNamespaces = Array.Empty<string>();

    private static readonly CSharpCompilationOptions SDefaultCompilationOptions =
        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(true).WithOptimizationLevel(OptimizationLevel.Release)
            .WithAllowUnsafe(true)
            .WithUsings(SDefaultNamespaces);

    public static Task Verify(string source)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var assemblyDirectoryPath = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
        var references = new List<PortableExecutableReference>
        {
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "mscorlib.dll")),
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "System.dll")),
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "System.Core.dll")),
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "System.Private.CoreLib.dll")),
            MetadataReference.CreateFromFile(Path.Join(assemblyDirectoryPath, "netstandard.dll")),
            MetadataReference.CreateFromFile(typeof(BitfieldAttribute).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location)
        };

        var compilation = CSharpCompilation.Create(
            "Tests",
            [syntaxTree],
            references,
            SDefaultCompilationOptions);

        var generator = new BitfieldsGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        using var stream = new MemoryStream();
        var emitResult = compilation.Emit(stream);
        if (!emitResult.Success)
            throw new Exception(string.Join("\n", emitResult.Diagnostics.Select(x => x.ToString())));

        driver = driver.RunGenerators(compilation);

        return Verifier.Verify(driver).UseDirectory("Cases");
    }
}