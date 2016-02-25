using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Newtonsoft.Json;

namespace RESTfulAD.Models
{
    public class ADObjectBase
    {
        //public List<string> AllowedAttributes
        //{
        //    get
        //    {
        //        return this.Properties.GetPropertyValues<string>(PropertyNameConstants.allowedAttributesEffective);
        //    }
        //}
        private List<string> AllowedAttributesEffective
        {
            get
            {
                return this.Properties.GetPropertyValues<string>(PropertyNameConstants.allowedAttributesEffective);
            }
        }

        public string ADSPath
        {
            get
            {
                return this.Properties.GetPropertyValue("adspath");
            }
        }

        public string CommonName
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.cn);
            }
        }
        public string Name
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.name);
            }
        }

        public string DisplayName
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.displayName);
            }
        }

        public string RDN
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.msDS_LastKnownRDN);
            }
        }

        public string ObjectCategory
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.objectCategory);
            }
        }

        public string DistinguishedName
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.distinguishedName);
            }
        }

        public string Alias
        {
            get
            {
                return this.Properties.GetPropertyValue(PropertyNameConstants.mailNickname);
            }
        }

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

        public Guid ObjectGuid
        {
            get
            {
                return Properties.GetPropertyValue(PropertyNameConstants.objectGUID, GuidConstructor);
            }
        }

        public List<string> ObjectClass
        {
            get
            {
                return this.Properties.GetPropertyValues<string>(PropertyNameConstants.objectClass);
            }
        }

        public DateTime WhenChanged
        {
            get
            {
                return this.Properties.GetPropertyValue<DateTime>(PropertyNameConstants.whenChanged);
            }
        }

        public DateTime WhenCreated
        {
            get
            {
                return this.Properties.GetPropertyValue<DateTime>(PropertyNameConstants.whenCreated);
            }
        }

        private bool resolvedObjectType = false;
        private ADObjectType _adObjType = ADObjectType.Unknown;

        public ADObjectType ObjectType
        {
            get
            {
                if (!resolvedObjectType) SetObjectType();
                return _adObjType;
            }
        }

        public bool CanIChangeProperty(string property)
        {
            return AllowedAttributesEffective?.Contains(property, StringComparer.OrdinalIgnoreCase) ?? false;
        }

        //public string[] QueriedProperties { get; set; } = new string[] { "AllowedAttributes", "AllowedAttributesEffective", "displayName" };

        [JsonIgnore]
        public PropertyCache Properties { get; private set; } = new PropertyCache();

        public ADObjectBase(DirectoryEntry de)
        {
            this.Properties = new PropertyCache(de);
        }

        public ADObjectBase(SearchResult sr)
        {
            this.Properties = new PropertyCache(sr);
        }

        public ADObjectBase(ADObjectBase adObj)
        {
            this.Properties = adObj.Properties;
        }

        internal ADObjectBase(PropertyCache pc)
        {
            this.Properties = pc;
        }

        internal ADObjectBase()
        {
            this.Properties = new PropertyCache();
        }

        public static Func<PropertyData, Guid> GuidConstructor { get; private set; } = (prop) => new Guid(prop.Value as byte[]);
        //public static Func<object, string> ProxyAddressParser { get; private set; } = (prop) => prop?.ToString().Substring(prop.ToString().IndexOf(":"));

        private void SetObjectType()
        {
            if (this.ObjectClass != null)
            {
                if (ObjectClass.Contains(ObjectClassNames.contact))
                    _adObjType = ADObjectType.Contact;

                if (ObjectClass.Contains(ObjectClassNames.group))
                    _adObjType = ADObjectType.Group;

                if (ObjectClass.Contains(ObjectClassNames.computer))
                    _adObjType = ADObjectType.Computer;

                if (_adObjType == ADObjectType.Unknown)
                {
                    if (ObjectClass.Contains(ObjectClassNames.user))
                        _adObjType = ADObjectType.User;
                }
            }

            resolvedObjectType = true;
        }
    }
}