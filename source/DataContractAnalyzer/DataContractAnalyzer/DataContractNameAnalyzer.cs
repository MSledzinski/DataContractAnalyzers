namespace DataContractAnalyzer
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DataContractNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DC0001";

        private const string Category = "DataContract";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.DataContractAnalyzerTitle), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.DataContractAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.DataContractAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeClassForDataContract, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeClassForDataContract(SyntaxNodeAnalysisContext context)
        {
            var declaration = context.Node as ClassDeclarationSyntax;

            var className = declaration.Identifier.ValueText;

            var dataContractAttribute =
                declaration.FindAttributeWithName(Constants.Attributes.DataCotnractAttributeName);

            if (dataContractAttribute == null)
            {
                return;
            }

            var nameParameter =
                dataContractAttribute.FindAttributeParamterWithName(
                    Constants.Attributes.DataCotnractAttributeNameParameterName);

            var nameParamterValue = nameParameter.GetAttributeParamterLiteralValue();

            if (nameParamterValue == className)
            {
                return;
            }

            var diagnostic = Diagnostic.Create(
                Rule, 
                context.Node.GetLocation(), 
                className);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
