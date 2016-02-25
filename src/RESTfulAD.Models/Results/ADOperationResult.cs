using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class ADOperationResult
    {
        public string ErrorMessage { get; set; }                
        public bool Success { get; set; }
    }
}