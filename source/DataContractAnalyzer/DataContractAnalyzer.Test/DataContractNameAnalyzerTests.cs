namespace DataContractAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestHelper;

    [TestClass]
    public class DataContractNameAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void ShouldNotGenerateDiagForEmpty()
        {
            var code = @"";

            VerifyCSharpDiagnostic(code);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagWhenNameAndDataContractEquals()
        {
            var code = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""ExampleDto"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                }
            }";

            VerifyCSharpDiagnostic(code);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagWhenNoNameParameterPresent()
        {
            var code = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                }
            }";

            VerifyCSharpDiagnostic(code);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagWhenNameParameterIsWritten()
        {
            var code = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Nam , Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                }
            }";

            VerifyCSharpDiagnostic(code);
        }

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
                Id = DataContractNameAnalyzer.DiagnosticId,
                Message = "Class name 'ExampleDto' is inconsistent with DataContract name parameter",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 6, 17)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new DataMemberPresenceCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new DataContractNameAnalyzer();
        }
    }
}