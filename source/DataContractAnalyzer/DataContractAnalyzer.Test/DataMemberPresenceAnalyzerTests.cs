﻿namespace DataContractAnalyzer.Test
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestHelper;

    [TestClass]
    public class DataMemberPresenceAnalyzerTests : CodeFixVerifier
    {
        [TestMethod]
        public void ShouldNotGenerateDiagForEmpty()
        {
            var code = @"";

            VerifyCSharpDiagnostic(code);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagWhenNoProperties()
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

        [TestMethod]
        public void ShouldNotGenerateDiagForExtensionObjectData()
        {
            var code = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract]
                public class Example : IExtensibleDataObject
                {
                    public ExtensionDataObject ExtensionData { get; set; }
                }
            }";

            VerifyCSharpDiagnostic(code);
        }

        [TestMethod]
        public void ShouldNotGenerateDiagForNotPublicProperties()
        {
            var code = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract]
                public class Example : IExtensibleDataObject
                {
                    protected string Data { get; set; }

                    private int Value { get; set; }
                }
            }";

            VerifyCSharpDiagnostic(code);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public void ShouldGenerateDiagForMissingDataMemberOnPublicProperty()
        {
            var test = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""Abc"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                    public string Value { get; set; }
                }
            }";

            var expected = new DiagnosticResult
                               {
                                   Id = DataMemberPresenceAnalyzer.DiagnosticId,
                                   Message = "DataMember attribute is missing",
                                   Severity = DiagnosticSeverity.Error,
                                   Locations =
                                       new[] {
                                                 new DiagnosticResultLocation("Test0.cs", 9, 21)
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
            return new DataMemberPresenceAnalyzer();
        }
    }
}