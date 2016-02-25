using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class PersonOrGroup : ADObjectBase
    {
        public string EmailAddress
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.mail);
            }
        }

        public List<string> EmailAddresses
        {
            get
            {
                return this.Properties.GetPropertyValues<string>(PropertyNameConstants.proxyAddresses);
            }
        }

        public string LegacyExchangeDN
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.legacyExchangeDN);
            }
        }

        public PersonOrGroup(DirectoryEntry de) : base(de)
        {
        }

        public PersonOrGroup(SearchResult sr) : base(sr)
        {
        }

        public PersonOrGroup(ADObjectBase adObj) : base(adObj)
        {
        }

        internal PersonOrGroup(PropertyCache pc) : base(pc)
        {
        }

        public PersonOrGroup() : base()
        {
        }
    }
}