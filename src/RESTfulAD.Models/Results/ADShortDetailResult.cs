using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class ADShortDetailResult
    {
        public int ResultCount { get; set; }
        public string ErrorMessage { get; set; }
        public Dictionary<string, Dictionary<string, string>> Results { get; private set; } = new Dictionary<string, Dictionary<string, string>>();

        public ADShortDetailResult(SearchResultCollection searchResults, List<string> props)
        {
            foreach (SearchResult result in searchResults)
            {
                var res = GetShortDetailFromSearchResult(result, props);
                this.Results.Add(res.Key, res.Value);
            }
            ResultCount = Results.Count;
        }

        public ADShortDetailResult(SearchResult result, List<string> props)
        {
            var res = GetShortDetailFromSearchResult(result, props);
            Results.Add(res.Key,res.Value);
            ResultCount = 1;
        }
        private KeyValuePair<string, Dictionary<string, string>> GetShortDetailFromSearchResult(SearchResult result, List<string> props)
        {
            PropertyCache pc = new PropertyCache(result);
            Dictionary<string, string> entry = new Dictionary<string, string>();
            foreach (var item in props)
            {
                var value = result.Properties[item];
                if (value != null)
                {
                    entry.Add(item, value.AsPropertyData().ToString());
                }
            }
            string DN = pc.GetPropertyValue<string>(PropertyNameConstants.distinguishedName);
            return new KeyValuePair<string, Dictionary<string, string>>(DN, entry);
        }

        public ADShortDetailResult()
        {
        }
    }
}