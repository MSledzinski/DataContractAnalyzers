namespace DataContractAnalyzer.Test
{
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TestHelper;

    [TestClass]
    public class DataMemberPresenceCodeFixTests : CodeFixVerifier
    {
        [TestMethod]
        public void ShouldApplyDataMemberAttribute()
        {
            var actualCode = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""ExampleDto"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
                    public string Value { get; set; }
                }
            }";

            ////NOTE - change whitespaces when 'fixer' improved
            var expectedCode = @"
            namespace DataContractAnalyzer.Test
            {
                using System.Runtime.Serialization;

                [DataContract(Name = ""ExampleDto"", Namespace = ""http://abc/com"")]
                public class ExampleDto
                {
        [DataMember]
        public string Value { get; set; }
                }
            }";

            VerifyCSharpFix(actualCode, expectedCode);
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