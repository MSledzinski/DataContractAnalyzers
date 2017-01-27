using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TestHelper;

namespace DataContractAnalyzer.Test
{
    [TestClass]
    public class DataContractAnalyzerTests : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void ShouldNotGenerateDiagForEmpty()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagWhenNameAndDataContractEquals()
        {
            var test = @"
namespace DataContractAnalyzer.Test
{
    using System.Runtime.Serialization;

    [DataContract(Name = ""ExampleDto"", Namespace = ""http://abc/com"")]
    public class ExampleDto
        {
        }
    }";
            VerifyCSharpDiagnostic(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void ShouldGenerateDiagForNameMismatch()
        {
            var test = @"
namespace DataContractAnalyzer.Test
{
    using System.Runtime.Serialization;

    [DataContract(Name = ""Abc"", Namespace = ""http://abc/com"")]
    public class ExampleDto
        {
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "DataContractAnalyzer",
                Message = "Class name 'ExampleDto' is inconsistent with DataContract name parameter",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 6, 5)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new DataContractAnalyzerCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DataContractNameAnalyzer();
        }
    }
}