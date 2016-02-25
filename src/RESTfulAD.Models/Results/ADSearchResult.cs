using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;

namespace RESTfulAD.Models
{
    public class ADSearchResult
    {
        public int ResultCount { get; set; }
        public string ErrorMessage { get; set; }
        public List<ADObjectBase> Results { get; private set; } = new List<ADObjectBase>();

        public ADSearchResult(SearchResultCollection searchResults, int pagesize = 0)
        {
            //this.ResultCount = pagesize;
            IEnumerator searchenumerator = searchResults.GetEnumerator();
            int count = 0;

            while (searchenumerator.MoveNext() && (pagesize < 0 || count < pagesize))
            {
                count++;
                this.Results.Add(((SearchResult)searchenumerator.Current).ToADObjectBaseInstance());
            }
            ResultCount = count;
        }

        public ADSearchResult(SearchResult result)
        {
            Results.Add(result.ToADObjectType());
        }

        public ADSearchResult()
        {
        }
    }
}