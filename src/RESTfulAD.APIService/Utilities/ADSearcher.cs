using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using RESTfulAD.Models;

namespace RESTfulAD.APIService
{
    public class ADSearcher
    {

        /// <summary>
        /// Default AD global catalog for searching
        /// </summary>
        private string DefaultGlobalCatalog = ADSConfig.Instance.DefaultGlobalCatalog;

        /// <summary>
        /// Active Directory properties used in this project
        /// </summary>
        public static readonly string[] AdProperties = new[]
            {
                "cn","objectClass", "displayname", "mail", "givenname", "sn", "telephoneNumber", "title",
                "physicalDeliveryOfficeName", "department", "distinguishedName", "manager", "directreports",
                "proxyAddresses", "mailNickName", "groupType", "member","memberof", "objectGUID", "objectSid",
                "whenChanged","whenCreated", PropertyNameConstants.msDS_LastKnownRDN, PropertyNameConstants.objectCategory,
                PropertyNameConstants.sAMAccountName, PropertyNameConstants.managedBy, PropertyNameConstants.msExchCoManagedByLink
            };

        /// <summary>
        /// DirectorySearcher for the class
        /// </summary>
        protected DirectorySearcher ds;

        public ADSearcher()
        {
            this.ds = ADSConfig.Instance.GetSearcher();
        }

        protected DirectorySearcher Ds
        {
            get
            {
                return this.ds;
            }
        }

        /// <summary>
        /// Escapes the given LDAP string by adding 
        /// a backslash in front of (, ), and \.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Escaped string</returns>
        protected static string EscapeLdap(string text)
        {
            var output = new StringBuilder();

            foreach (char ch in text)
            {
                switch (ch)
                {
                    case '(':
                        output.Append("\\28");
                        break;
                    case ')':
                        output.Append("\\29");
                        break;
                    case '\\':
                        output.Append("\\5c");
                        break;
                    default:
                        output.Append(ch);
                        break;
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Gets the value of the given property from the given search result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="property">The property to look for in the result.</param>
        /// <returns>If found, property value cast to a string; otherwise empty string</returns>
        protected static T GetResultProperty<T>(SearchResult result, string property)
        {
            // Use the property argument to index into Properties, which is 
            // a ResultPropertyCollection. Such indexing is supported in C#.
            // Actually using the Item property of the ResultPropertyCollection.
            if (result != null && result.Properties[property] != null &&
                result.Properties[property].Count > 0)
            {
                return (T)result.Properties[property][0];
            }

            // Return empty (instead of NULL) because ASPX Literal controls can't deal with null
            return default(T);
        }

        protected static List<T> GetResultProperties<T>(SearchResult result, string property)
        {
            // Use the property argument to index into Properties, which is 
            // a ResultPropertyCollection. Such indexing is supported in C#.
            // Actually using the Item property of the ResultPropertyCollection.
            if (result != null && result.Properties[property] != null &&
                result.Properties[property].Count > 0)
            {
                return result.Properties[property].OfType<T>().ToList();
            }

            // Return empty (instead of NULL) because ASPX Literal controls can't deal with null
            return new List<T>();
        }


        /// <summary>
        /// Create Person object from search result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Person object or null</returns>
        protected User PersonFromSearchResult(SearchResult result)
        {
            return new User(result);
        }

        protected Group GroupFromSearchResult(SearchResult result)
        {
            if (result == null)
            {
                return null;
            }

            var group = new Group(result);
            
            return group;
        }        
               

    }
}
