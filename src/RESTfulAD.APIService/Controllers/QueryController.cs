using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RESTfulAD.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RESTfulAD.APIService
{
    [Route("api/[controller]")]
    public class QueryController : Controller
    {
        SearchUtility su = new SearchUtility();

        [HttpGet("{filter}/anr")]
        public ADSearchResult SearchAnr(string filter)
        {            
            return su.ANRSearch(filter, ADSConfig.Instance.MaxSearchResult);

        }

        [HttpGet("{filter}")]
        public ADSearchResult Search(string filter)
        {
            return su.ANRSearch(filter, ADSConfig.Instance.MaxSearchResult);
        }

        [HttpGet("/api/mygroups")]
        public ADSearchResult MyGroups()
        {
            return su.MyGroups(Utilities.GetSAMAccountName(User), false, ADSConfig.Instance.MaxSearchResult);
        }

        [HttpGet("/api/mygroups/{filter}/search")]
        public ADSearchResult MyGroups(string filter)
        {
            return su.MyGroups(Utilities.GetSAMAccountName(User), filter, false, ADSConfig.Instance.MaxSearchResult);
        }

        [HttpPost("getdisplayandemail")]
        public ADShortDetailResult Post([FromBody]List<string> DNs)
        {
            return su.GetDetailsForDNs(DNs, new string[] { PropertyNameConstants.displayName, PropertyNameConstants.mail, PropertyNameConstants.mailNickname }.ToList());
        }        
    }
}
