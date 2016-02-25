using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class Contact : Person
    {
        public Contact(DirectoryEntry de) : base(de)
        {
        }

        public Contact(SearchResult sr) : base(sr)
        {
        }

        public Contact(ADObjectBase adObj) : base(adObj)
        {
        }

        internal Contact(PropertyCache pc) : base(pc)
        {
        }

        public Contact() : base()
        {
        }
    }
}