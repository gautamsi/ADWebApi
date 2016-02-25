using System;

namespace RESTfulAD.Models
{
    public class AttributeMetadata
    {
        public string Name { get; set; }
        public TypeCode TypeCode { get; set; }
        public bool IsSingleValued { get; set; } = true;
    }
}