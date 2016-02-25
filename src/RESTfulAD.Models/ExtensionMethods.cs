using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace RESTfulAD.Models
{
    public static class ExtensionMethods
    {

        public static bool throwError = false;
        private const string malformedDN = "input not appears to be distinguishedName";

        /// <summary>
        /// get the domain name in fqdn format from distinguishedName
        /// </summary>
        /// <param name="distinguishedName"> DN string</param>
        /// <param name="throwErr"> to throw error or simply return null; default: false</param>
        /// <returns></returns>
        public static string GetDomainFQDNfromDN(this string distinguishedName, bool throwErr = false)
        {
            if (distinguishedName.IsNullOrEmpty())
            {
                return throwErrOrNullForArgument(throwErr, malformedDN);
            }
            string domain = null;
            int index = distinguishedName.IndexOf("DC=", StringComparison.OrdinalIgnoreCase);
            var domainPart = distinguishedName;
            while (index >= 0)
            {
                domainPart = domainPart.Substring(index + 3);       // exclude DC=
                index = domainPart.IndexOf("DC=", StringComparison.OrdinalIgnoreCase);
                var comaIndex = domainPart.IndexOf(",");

                if ((comaIndex > 0 && index != comaIndex + 1) || (comaIndex == -1 && index >= 0))
                {
                    return throwErrOrNullForArgument(throwErr, malformedDN);
                }

                domain += domainPart.Substring(0, comaIndex >= 0 ? comaIndex : domainPart.Length);
                if (index > 0)
                {
                    domain += ".";
                }

            }

            if (domain.EndsWith("."))
            {
                return throwErrOrNullForArgument(throwErr, malformedDN);
            }
            return domain;
        }

        private static string throwErrOrNullForArgument(bool throwErr, string msg)
        {
            if (throwErr) throw new ArgumentException(msg);
            return null;
        }

        /// <summary>
        /// Get Domain distinguishenName from given fqn
        /// </summary>
        /// <param name="fqdn"> domain fqdn to parse</param>
        /// <returns></returns>
        public static string GetDomainDNfromFQDN(this string fqdn)
        {
            if (fqdn.IsNullOrEmpty())
            {
                throw new ArgumentNullException("fqdn");
            }
            var dn = string.Empty;

            foreach (string part in fqdn.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dn += "DC=" + part + ',';
            }
            dn = dn.TrimEnd(',');
            return dn;
        }

        /// <summary>
        /// check if given string is null or empty
        /// </summary>
        /// <param name="str">string to check</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string xFormat(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string GetPropertyValue(this PropertyCache props, string name)
        {
            return props.GetPropertyValue<string>(name);
            //if (props != null && props[name] != null &&
            //    props[name].Count > 0)
            //{
            //    return (string)props[name].Value;
            //}

            //// default value of Type
            //return string.Empty;

        }

        public static T GetPropertyValue<T>(this PropertyCache propCache, string propName,
            Func<PropertyData, T> helperAction = null)
        {
            Type t = typeof(T);

            if (propCache != null && propCache[propName] != null &&
                propCache[propName].Count > 0)
            {
                return helperAction != null ? helperAction(propCache[propName]) : (T)propCache[propName][0];
            }

            // default value of Type
            return default(T);
        }

        public static List<T> GetPropertyValues<T>(this PropertyCache propCache, string propName,
            Func<object, T> helperAction = null)
        {
            if (propCache != null && propCache[propName] != null &&
                propCache[propName].Count > 0)
            {
                List<T> values = new List<T>();
                foreach (var item in propCache[propName].Values)
                {
                    values.Add(helperAction != null ? helperAction(item) : (T)item);
                }
                return values;
            }

            // default value of Type
            return default(List<T>);
        }

        internal static PropertyData AsPropertyData(this object value)
        {
            return new PropertyData(value);
        }

        public static ADObjectBase ToDerivedtype(this ADObjectBase ado)
        {
            switch (ado.ObjectType)
            {
                case ADObjectType.User:
                    return new User(ado);
                case ADObjectType.Contact:
                    return new Contact(ado);
                case ADObjectType.Group:
                    return new Group(ado);
                case ADObjectType.Computer:
                case ADObjectType.OU:
                case ADObjectType.Domain:
                case ADObjectType.Schema:
                case ADObjectType.Custom:
                case ADObjectType.Unknown:
                default:
                    return ado;
            }

            //string objectClass = ado.Properties.GetPropertyValue(PropertyNameConstants.sAMAccountType);
            //SAMAccountType sAMAccountType = ado.Properties.GetPropertyValue<SAMAccountType>(PropertyNameConstants.sAMAccountType);

            //if((sAMAccountType & SAMAccountType.SAM_NORMAL_USER_ACCOUNT)  == SAMAccountType.SAM_NORMAL_USER_ACCOUNT)
            //    return new User(ado);

            //if (sAMAccountType.HasFlag(SAMAccountType.SAM_GROUP_OBJECT) || sAMAccountType.HasFlag(SAMAccountType.SAM_NORMAL_USER_ACCOUNT))
            //    return new Group(ado);

            //if ((sAMAccountType & SAMAccountType.SAM_NORMAL_USER_ACCOUNT) == SAMAccountType.SAM_NORMAL_USER_ACCOUNT)
            //    return new Contact(ado);



            //return ado;
        }
        public static ADObjectBase ToADObjectType(this DirectoryEntry entry)
        {
            return new ADObjectBase(entry).ToDerivedtype();
        }
        public static ADObjectBase ToADObjectType(this SearchResult result)
        {
            return new ADObjectBase(result).ToDerivedtype();
        }

        public static ADObjectBase ToADObjectBaseInstance(this SearchResult result)
        {
            return new PropertyCache(result).ToADObjectBaseInstance();
        }

        public static ADObjectBase ToADObjectBaseInstance(this DirectoryEntry de)
        {
            return new PropertyCache(de).ToADObjectBaseInstance();
        }
        //public static List<ADObjectBase> ToADObjectBaseInstances(this SearchResultCollection results)
        //{
        //    foreach (SearchResult item in SearchResultCollection)
        //    {

        //    }
            
        //    return new PropertyCache(result).ToADObjectBaseInstance();
        //}

        public static ADObjectType GetADObjectType(this PropertyCache properties)
        {
            var objectClass = properties.GetPropertyValues<string>(PropertyNameConstants.objectClass);
            ADObjectType _adObjType = ADObjectType.Unknown;
            if (objectClass != null)
            {
                if (objectClass.Contains(ObjectClassNames.contact))
                    _adObjType = ADObjectType.Contact;


                if (objectClass.Contains(ObjectClassNames.group))
                    _adObjType = ADObjectType.Group;

                if (objectClass.Contains(ObjectClassNames.computer))
                    _adObjType = ADObjectType.Computer;

                if (_adObjType == ADObjectType.Unknown)
                {
                    if (objectClass.Contains(ObjectClassNames.user))
                        _adObjType = ADObjectType.User;
                }

            }
            return _adObjType;
        }

        public static ADObjectBase ToADObjectBaseInstance(this PropertyCache pc)
        {
            ADObjectType adoType = pc.GetADObjectType();

            switch (adoType)
            {
                case ADObjectType.User:
                    return new User(pc);
                case ADObjectType.Contact:
                    return new Contact(pc);
                case ADObjectType.Group:
                    return new Group(pc);
                case ADObjectType.Computer:
                case ADObjectType.OU:
                case ADObjectType.Domain:
                case ADObjectType.Schema:
                case ADObjectType.Custom:
                case ADObjectType.Unknown:
                default:
                    return new ADObjectBase(pc);
            }
        }

    }

}
