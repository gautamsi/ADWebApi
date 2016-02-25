using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RESTfulAD.Models;

namespace RESTfulAD.APIService
{
    public class Utilities
    {
        internal static string GetSAMAccountName(ClaimsPrincipal User)
        {
            var user = User.Identity.Name.IsNullOrEmpty() ? ADSConfig.Instance.UserName : User.Identity.Name;
            if (user.Contains("\\"))
            {
                user = user.Split(new string[] { "\\" }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            return user;
        }
    }
}
