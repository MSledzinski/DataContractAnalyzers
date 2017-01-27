using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DataContractAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class DataContractNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "DataContractNameAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
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

            var className = declaration.Identifier.ValueText;

            var dataContractAttribute =
                declaration
                .AttributeLists
                .SelectMany(d => d.Attributes)
                .FirstOrDefault(d => (d.Name as IdentifierNameSyntax).Identifier.ValueText == "DataContract");

            if (dataContractAttribute == null)
            {
                return;
            }

            var nameParameter = dataContractAttribute.ArgumentList.Arguments.FirstOrDefault(a => a.NameEquals.Name.Identifier.ValueText == "Name");
            
            if (nameParameter == null)
            {
                return;
            }

            if ((nameParameter.Expression as LiteralExpressionSyntax).Token.ValueText == className)
            {
                return;
            }

            // For all such symbols, produce a diagnostic.
            var diagnostic = Diagnostic.Create(
                Rule, 
                context.Node.GetLocation(), 
                className);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
