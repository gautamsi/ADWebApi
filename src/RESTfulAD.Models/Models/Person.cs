using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class Person : ADObjectBase
    {
        //public bool IsExchangeRecipient { get; set; }
        //public string Alias
        //{
        //    get
        //    {
        //        return this.Properties.GetPropertyValue(PropertyNameConstants.mailNickname);
        //    }
        //}

        //public string EmailAddress
        //{
        //    get
        //    {
        //        return this.Properties.GetPropertyValue(PropertyNameConstants.mail);
        //    }
        //}

        //public List<string> EmailAddresses
        //{
        //    get
        //    {
        //        return this.Properties.GetPropertyValues<string>(PropertyNameConstants.proxyAddresses);
        //    }
        //}

        //public string LegacyExchangeDN
        //{
        //    get
        //    {
        //        return this.Properties.GetPropertyValue(PropertyNameConstants.legacyExchangeDN);
        //    }
        //}

        public string ExternalEmailAddress
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.targetAddress);
            }
        }

        public object RecipientType
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.msExchRecipientDisplayType);
            }
        }

        public string FirstName
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.givenName);
            }
        }

        public string LastName
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.sn);
            }
        }

        public string Title
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.title);
            }
        }

        public string Office
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.physicalDeliveryOfficeName);
            }
        }

        public Person(DirectoryEntry de) : base(de)
        {
        }

        public Person(SearchResult sr) : base(sr)
        {
        }

        public Person(ADObjectBase adObj) : base(adObj)
        {
        }

        internal Person(PropertyCache pc) : base(pc)
        {
        }

        public Person() : base()
        {
        }
    }
}