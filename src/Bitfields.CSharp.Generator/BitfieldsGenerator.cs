using System.Text;
using Bitfields.CSharp.Generator.Generating;
using Bitfields.CSharp.Generator.Parsing;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Bitfields.CSharp.Generator;

[Generator]
public class BitfieldsGenerator : IIncrementalGenerator
{
    private const string BitfieldAttributeMarker = "Bitfields.CSharp.BitfieldAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        var bitfieldAttributesSources = initContext.SyntaxProvider.ForAttributeWithMetadataName(
            BitfieldAttributeMarker,
            static (_, _) => true,
            static (context, _) => context
        );

        initContext.RegisterSourceOutput(bitfieldAttributesSources, Emit);
    }

    private static void Emit(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
    {
        var classSymbol = (INamedTypeSymbol)source.TargetSymbol;
        var classNode = (TypeDeclarationSyntax)source.TargetNode;
        if (!classNode.Modifiers.Any(SyntaxKind.PartialKeyword))
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Create(
                    "BITFS001",
                    "Not a partial class or struct",
                    "{0} must be marked partial"),
                classNode.Identifier.GetLocation(), classSymbol.Name));
            return;
        }

        var type = classSymbol.TypeKind;
        var visibility = classNode.Modifiers.FirstOrDefault(m => m.IsKind(SyntaxKind.PublicKeyword) || m.IsKind(SyntaxKind.InternalKeyword) || m.IsKind(SyntaxKind.PrivateKeyword) || m.IsKind(SyntaxKind.ProtectedKeyword));
        var bitfieldAttributeData = GetBitfieldsAttribute(source);
        var bitfield = BitfieldParser.ParseBitfield(bitfieldAttributeData, classSymbol.Name, visibility.ToString());
        var membersList = classNode.Members.ToList();
        membersList.Reverse();
        var fieldsParseResults = BitfieldFieldsParser.ParseBitfieldFields(bitfield, membersList);
        if (fieldsParseResults.Error != null)
        {
            context.ReportDiagnostic(Diagnostic.Create(fieldsParseResults.Error.Descriptor,
                fieldsParseResults.Error.Location));
            return;
        }

        var fieldBitsSum = fieldsParseResults.Fields.Sum(f => f.Bits);
        var bitfieldBits = InternalTypeUtil.GetBitsFromInternalType(bitfield.Type);
        if (fieldBitsSum < bitfieldBits)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Create(
                    "BITFS006",
                    "Field bits are less than the bitfield bits",
                    $"The total number of bits of the fields ({fieldBitsSum} bits) is less than the bits of the bitfield type '{bitfield.Type.ToTypeString()}' ({bitfieldBits} bits), you can add a padding field (suffixed with '_') to fill the remaining '{bitfieldBits - fieldBitsSum}' bits."),
                classNode.Identifier.GetLocation(), classSymbol.Name));
            return;
        }

        if (fieldBitsSum > bitfieldBits)
        {
            context.ReportDiagnostic(Diagnostic.Create(DiagnosticDescriptors.Create(
                    "BITFS007",
                    "Field bits are greater than the bitfield bits",
                    $"The total number of bits of the fields ({fieldBitsSum} bits) is greater than the number of bits of the bitfield type '{bitfield.Type.ToTypeString()}' ({bitfieldBits} bits)."),
                classNode.Identifier.GetLocation(), classSymbol.Name));
            return;
        }

        var fieldConstantsSource = FieldConstGetterSetter.GenerateFieldConstantsSource(bitfield, fieldsParseResults.Fields);
        var fieldGettersSource = FieldConstGetterSetter.GenerateFieldGettersSource(bitfield, fieldsParseResults.Fields);
        var fieldSettersSource = FieldConstGetterSetter.GenerateFieldSettersSource(bitfield, fieldsParseResults.Fields);
        var fromBitsSource = FromIntoBits.FromBitsSource(bitfield, fieldsParseResults.Fields);
        var fromBitsWithDefaultsSource = FromIntoBits.FromBitsWithDefaultsSource(bitfield, fieldsParseResults.Fields);
        var toBitsSource = FromIntoBits.ToBitsSource(bitfield, fieldsParseResults.Fields);
        var settingFieldDefaultsSource = Common.GenerateSettingDefaultsSource(fieldsParseResults.Fields);
        var constructorSource = Constructor.GeneratorConstructorSource(bitfield, fieldsParseResults.Fields);
        var builderSource = Builder.BuilderSource(bitfield, fieldsParseResults.Fields);
        var toBuilderSource = Builder.ToBuilderSource(bitfield);
        var nameSpace = GetFullNamespace(classSymbol).Trim().Replace(" ", "");

        var classImpl = $$"""
                          {{bitfield.Visibility}} partial {{type.ToString().ToLower()}} {{classSymbol.Name}} {
                             {{fieldConstantsSource}}
                             {{constructorSource}}
                             {{fieldGettersSource}}
                             {{fieldSettersSource}}
                             {{fromBitsSource}}
                             {{fromBitsWithDefaultsSource}}
                             {{toBitsSource}}
                             {{settingFieldDefaultsSource}}
                             {{builderSource}}
                             {{toBuilderSource}}
                          }
                          """;

        if (string.IsNullOrEmpty(nameSpace))
        {
            context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From($$"""
                                                                            {{classImpl}}
                                                                            """, Encoding.UTF8));
        } else {
            context.AddSource($"{classSymbol.Name}.g.cs", SourceText.From($$"""
                                                                            namespace {{nameSpace}}
                                                                            {
                                                                                {{classImpl}}
                                                                            }
                                                                            """, Encoding.UTF8));
        }
    }

    private static AttributeData GetBitfieldsAttribute(GeneratorAttributeSyntaxContext source)
    {
        return source.Attributes.First(data => data.AttributeClass?.ToString() == BitfieldAttributeMarker);
    }

    private static string GetFullNamespace(INamedTypeSymbol symbol)
    {
        if (symbol == null) throw new ArgumentNullException(nameof(symbol));

        var namespaces = new Stack<string>();

        // Traverse up the namespace tree
        var currentNamespace = symbol.ContainingNamespace;
        while (currentNamespace != null && !currentNamespace.IsGlobalNamespace)
        {
            namespaces.Push(currentNamespace.Name);
            currentNamespace = currentNamespace.ContainingNamespace;
        }

        return string.Join(".", namespaces);
    }
}