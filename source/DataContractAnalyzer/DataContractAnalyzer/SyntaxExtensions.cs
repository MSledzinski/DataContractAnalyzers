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
            return @this.AttributeLists.FindAttributeWithName(name);
        }

        public static AttributeSyntax FindAttributeWithName(
            this ClassDeclarationSyntax @this, 
            string name)
        {
            return @this.AttributeLists.FindAttributeWithName(name);
        }

        public static AttributeSyntax FindAttributeWithName(
            this SyntaxList<AttributeListSyntax> @this,
            string name)
        {
            return @this
                  .SelectMany(d => d.Attributes)
                  .FirstOrDefault(d => HasName(d, name));
        }

        public static AttributeArgumentSyntax FindAttributeParamterWithName(
            this AttributeSyntax @this,
            string name)
        {
            return @this?
                    .ArgumentList?
                    .Arguments
                    .FirstOrDefault(a => HasParamterName(a, name));
        }

        public static string GetAttributeParamterLiteralValue(
            this AttributeArgumentSyntax @this)
        {
            var literalExpresion = @this.Expression as LiteralExpressionSyntax;

            return literalExpresion != null ? literalExpresion.Token.ValueText : string.Empty;
        }

        public static bool IsPublicProperty(this PropertyDeclarationSyntax @this)
        {
            return @this.Modifiers.Any(m => m.ValueText == "public");
        }

        public static string GetSimpleTypeName(this PropertyDeclarationSyntax @this)
        {
            var typeName = @this.Type as IdentifierNameSyntax;

            return typeName != null ? typeName.Identifier.ValueText : string.Empty;
        }

        private static bool HasName(AttributeSyntax syntax, string name)
        {
            var identifier = syntax.Name as IdentifierNameSyntax;
            return identifier != null && identifier.Identifier.ValueText == name;
        }

        private static bool HasParamterName(AttributeArgumentSyntax syntax, string name)
        {
            if (syntax.NameEquals == null)
            {
                return false;
            }

            return syntax.NameEquals.Name.Identifier.ValueText == name;
        }
    }
}