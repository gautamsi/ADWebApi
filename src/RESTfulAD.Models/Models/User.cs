using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class User : Person
    {
        public User(DirectoryEntry de) : base(de)
        {
        }

        public User(SearchResult sr) : base(sr)
        {
        }

        public User(ADObjectBase adObj) : base(adObj)
        {
        }

        internal User(PropertyCache pc) : base(pc)
        {
        }

        public User() : base()
        {
        }
    }
}