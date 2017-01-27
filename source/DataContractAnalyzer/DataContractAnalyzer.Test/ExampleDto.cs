namespace DataContractAnalyzer.Test
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ExampleDto
    {
        [DataMember]
        public int Value { get; set; }
    }
}