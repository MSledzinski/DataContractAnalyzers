namespace DataContractAnalyzer
{
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DataMemberPresenceCodeFixProvider)), Shared]
    public class DataMemberPresenceCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds 
            => ImmutableArray.Create(DataMemberPresenceAnalyzer.DiagnosticId);
        
        public sealed override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().First();

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Resources.DataMemberFixTitle,
                    createChangedSolution: c => MakeUppercaseAsync(context.Document, declaration, c),
                    equivalenceKey: nameof(DataMemberPresenceCodeFixProvider)),
                diagnostic);
        }

        private async Task<Solution> MakeUppercaseAsync(Document document, PropertyDeclarationSyntax modelDeclarationSyntax, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);

            var memberAttribute =
                modelDeclarationSyntax.AttributeLists.Add(
                    SyntaxFactory.AttributeList(
                        SyntaxFactory.SingletonSeparatedList<AttributeSyntax>(
                            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("DataMember")))));

            return
                document.WithSyntaxRoot(root.ReplaceNode(modelDeclarationSyntax,
                    modelDeclarationSyntax.WithAttributeLists(memberAttribute))).Project.Solution;
        }
    }
}