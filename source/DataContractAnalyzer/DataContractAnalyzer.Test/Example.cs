namespace DataContractAnalyzer.Test
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Example : IExtensibleDataObject
    {
        public string Data { get; set; }

        public ExtensionDataObject ExtensionData { get; set; }
    }
}