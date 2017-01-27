namespace DataContractAnalyzer
{
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class SyntaxExtensions
    {
        public static AttributeSyntax FindAttributeWithName(
            this PropertyDeclarationSyntax @this, 
            string name)
        {
            return @this
               .AttributeLists
               .SelectMany(d => d.Attributes)
               .FirstOrDefault(d => (d.Name as IdentifierNameSyntax).Identifier.ValueText == name);
        }

        public static AttributeSyntax FindAttributeWithName(
            this ClassDeclarationSyntax @this, 
            string name)
        {
            return @this
               .AttributeLists
               .SelectMany(d => d.Attributes)
               .FirstOrDefault(d => (d.Name as IdentifierNameSyntax).Identifier.ValueText == name);
        }
    }
}