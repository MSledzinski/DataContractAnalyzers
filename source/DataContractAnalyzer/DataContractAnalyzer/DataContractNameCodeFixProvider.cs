using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DataContractAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DataContractNameCodeFixProvider)), Shared]
    public class DataContractNameCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds 
            => ImmutableArray.Create(DataContractNameAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root
                                .FindToken(diagnosticSpan.Start)
                                .Parent
                                .AncestorsAndSelf()
                                .OfType<ClassDeclarationSyntax>()
                                .First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Resources.DataContractFixTitle,
                    createChangedSolution: c => ChangeNameParameterInAttribute(context.Document, declaration, c),
                    equivalenceKey: nameof(DataContractNameCodeFixProvider)),
                diagnostic);
        }

        private async Task<Solution> ChangeNameParameterInAttribute(
            Document document, 
            ClassDeclarationSyntax modelDeclarationSyntax, 
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            var className = modelDeclarationSyntax.Identifier.ValueText;

            var dataContractAttribute =
              modelDeclarationSyntax.FindAttributeWithName(Constants.Attributes.DataCotnractAttributeName);

            var nameParameter =
             dataContractAttribute.FindAttributeParamterWithName(
                 Constants.Attributes.DataCotnractAttributeNameParameterName);
            
            var fix =
                CreateFixedAttribute(nameParameter, className);

            return
                document.WithSyntaxRoot(
                    root.ReplaceNode(nameParameter,fix)).Project.Solution;
        }

        private static AttributeArgumentSyntax CreateFixedAttribute(AttributeArgumentSyntax nameParameter, string className)
        {
            return nameParameter
                        .WithExpression(
                            SyntaxFactory.LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                SyntaxFactory.Token(
                                    SyntaxTriviaList.Empty, 
                                    SyntaxKind.StringLiteralToken, 
                                    $@"""{className}""", 
                                    $@"""{className}""",
                                    SyntaxTriviaList.Empty)));
        }
    }
}