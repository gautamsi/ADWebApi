using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.APIService
{
    public static class ExtensionMethods
    {
        public static string ToOperatorString(this LDAPFilterOperator oper)
        {
            switch (oper)
            {
                case LDAPFilterOperator.Equal:
                    return "=";
                case LDAPFilterOperator.NotEqual:
                    return "=";
                case LDAPFilterOperator.GreaterThan:
                    return ">";
                case LDAPFilterOperator.LessThan:
                    return "<";
                default:
                    break;
            }
            return "=";
        }

        public static IEnumerable<char> GetDomainFQDNfromDN(string value)
        {
            throw new NotImplementedException();
        }
    }
}
