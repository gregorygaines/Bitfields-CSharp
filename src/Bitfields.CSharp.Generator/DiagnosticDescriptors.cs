using Microsoft.CodeAnalysis;

namespace Bitfields.CSharp.Generator;

public static class DiagnosticDescriptors
{
    public static DiagnosticDescriptor Create(string id, string title, string messageFormat)
    {
        return new DiagnosticDescriptor(
            id,
            title,
            messageFormat,
            "Bitfields.CSharp.Generator",
            DiagnosticSeverity.Error,
            true);
    }
}