using System.Reflection;

namespace Bitfields.CSharp.Tests.Snapshot;

public static class CaseLoader
{
    private static readonly string[] CaseNames;

    static CaseLoader()
    {
        CaseNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
    }

    public static Task<string> LoadCase(string caseName)
    {
        var fullName = CaseNames.FirstOrDefault(s => s.EndsWith($".{caseName}.cs"));
        if (fullName is null) throw new Exception($"Can't find the test case: {caseName}");
        using var stream = typeof(CaseLoader).Assembly.GetManifestResourceStream(fullName);
        if (stream is null) throw new Exception("Null stream when loading test cases.");

        return new StreamReader(stream).ReadToEndAsync();
    }
}