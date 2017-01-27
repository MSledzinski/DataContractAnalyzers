namespace DataContractAnalyzer
{
    using System.Collections.Immutable;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DataMemberPresenceAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DataMemberPresenceAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.MembersAnalyzerTitle), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.MemberMessageFormat), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.MemberDescription), Resources.ResourceManager, typeof(Resources));

        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeClassForDataContract, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassForDataContract(SyntaxNodeAnalysisContext context)
        {
            var declaration = context.Node as ClassDeclarationSyntax;

            var dataContractAttribute = declaration.FindAttributeWithName("DataContract");

            if (dataContractAttribute == null)
            {
                return;
            }

        var propertyDeclarations = declaration.DescendantNodes().OfType<PropertyDeclarationSyntax>();

            foreach (var propertyDeclarationSyntax in propertyDeclarations)
            {
                // skip not public
                if (propertyDeclarationSyntax.Modifiers.All(m => m.ValueText != "public"))
                {
                    continue;
                }

                // skip for type ExtensionDataObject
                var typeName = (propertyDeclarationSyntax.Type as IdentifierNameSyntax);

                if (typeName != null && typeName.Identifier.ValueText == "ExtensionDataObject")
                {
                    continue;
                }

                var memberAttributeSyntax = propertyDeclarationSyntax.FindAttributeWithName("DataMember");

                if (memberAttributeSyntax == null)
                {
                    var diagnostic = Diagnostic.Create(Rule, propertyDeclarationSyntax.GetLocation());

                    context.ReportDiagnostic(diagnostic);

                    return;
                }
            }
        }

    }
}