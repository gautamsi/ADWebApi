using System.Collections.Generic;

namespace RESTfulAD.Models
{
    public class GroupContext
    {
        public string QueryString { get; set; }
        public List<Group> Results { get; set; }
        public int ResultCount { get; set; }
    }
}