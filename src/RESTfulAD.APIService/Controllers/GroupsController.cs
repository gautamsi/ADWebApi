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
    public class GroupsController : Controller
    {
        SearchUtility su = new SearchUtility();

        [HttpGet("{groupEmail}")]
        public Group Get(string groupEmail, [FromQuery] bool member = false)
        {
            return su.FindGroupBySMTPAddress(groupEmail, member);
        }

        [HttpGet("{groupEmail}/search")]
        public ADSearchResult SearchGroup(string groupEmail, [FromQuery] bool member = false)
        {
            return su.SearchGroup(groupEmail, member, ADSConfig.Instance.MaxSearchResult);
        }
        [HttpGet("{groupDN}/{operation}/{memberDN}")]
        public ADOperationResult PerformOperation(string groupDN, string operation, string memberDN)
        {
            switch (operation)
            {
                case "add":
                    return su.AddMemberToGroup(groupDN,memberDN, Utilities.GetSAMAccountName(User));
                case "remove":
                    return su.RemoveMemberFromGroup(groupDN, memberDN, Utilities.GetSAMAccountName(User));

                default:
                    return new ADOperationResult() { ErrorMessage = "Did not recognize requested operation:" + operation };
            }

        }        
    }
}
