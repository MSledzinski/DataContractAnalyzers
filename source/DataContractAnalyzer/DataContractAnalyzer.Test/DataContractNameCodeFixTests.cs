namespace DataContractAnalyzer.Test
{
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestHelper;

    [TestClass]
    public class DataContractNameCodeFixTests : CodeFixVerifier
    {
        [TestMethod]
        public void ShouldSetupProperName()
        {
            var actualCode = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""WrongName"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                }
            }";

            // Note - change whitespaces when 'fixer' improved
            var expectedCode = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""ExampleDto"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                }
            }";

            VerifyCSharpFix(actualCode, expectedCode);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new DataContractNameCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DataContractNameAnalyzer();
        }
    }
}