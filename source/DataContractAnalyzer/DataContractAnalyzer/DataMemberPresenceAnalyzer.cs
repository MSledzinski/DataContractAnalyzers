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
        public const string DiagnosticId = "DM0001";

        private const string Category = "DataContract";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.DataMembersAnalyzerTitle), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.DataMemberMessageFormat), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.DataMemberDescription), Resources.ResourceManager, typeof(Resources));

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

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
                if (propertyDeclarationSyntax.IsPublicProperty() == false)
                {
                    continue;
                }

                if (propertyDeclarationSyntax.GetSimpleTypeName() == "ExtensionDataObject")
                {
                    continue;
                }

                var memberAttributeSyntax =
                    propertyDeclarationSyntax.FindAttributeWithName("DataMember");

                if (memberAttributeSyntax != null)
                {
                    continue;
                }

                var diagnostic = Diagnostic.Create(Rule, propertyDeclarationSyntax.GetLocation());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}