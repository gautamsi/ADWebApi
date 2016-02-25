using RESTfulAD.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.APIService
{
    public class SearchUtility : ADSearcher
    {


        public SearchUtility() : base()
        {

        }

        /// <summary>
        /// Finds the person context by searching in Active Directory 
        /// given the SMTP address.
        /// </summary>
        /// <param name="SMTPAddress">The SMTP address.</param>
        /// <returns>PersonContext object or null if not found.</returns>
        public ADObjectBase FindADObjectBySMTPAddress(string SMTPAddress)
        {
            if (!string.IsNullOrWhiteSpace(SMTPAddress))
            {
                // Specify AdProperties as the properties to retrieve 
                // from Active Directory during the search.
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);

                // Set the LDAP format filter string, since we're using LDAP as the
                // service provider for Active Directory Domain Services.
                Ds.Filter = "(|(mail=" + SMTPAddress + ")(proxyaddresses=smtp:" + SMTPAddress + "))";// (objectcategory=Person))";


                // Execute search in Active Directory and
                // get the first SearchResult object.
                SearchResult result = Ds.FindOne();

                // Create and initialize Person object from the 
                // SearchResult object.
                return result.ToADObjectBaseInstance(); ;
            }

            return null;
        }
        public User FindUserByLogonName(string userName)
        {


            if (!string.IsNullOrWhiteSpace(userName))
            {
                string ldap = "";
                if (userName.Contains("@"))
                {
                    ldap = string.Format("(&(objectCategory=User)(userprincipalName={0}))", EscapeLdap(userName));
                }
                else
                {

                    ldap = string.Format("(&(objectCategory=User)(samAccountName={0}))", EscapeLdap(userName));
                }
                // Specify AdProperties as the properties to retrieve 
                // from Active Directory during the search.
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);

                // Set the LDAP format filter string, since we're using LDAP as the
                // service provider for Active Directory Domain Services.
                Ds.Filter = ldap;


                // Execute search in Active Directory and
                // get the first SearchResult object.
                SearchResult result = Ds.FindOne();

                // Create and initialize Person object from the 
                // SearchResult object.
                return (User)result.ToADObjectType(); ;
            }

            return null;
        }


        public ADOperationResult AddMemberToGroup(string groupDN, string memberDN, string requester)
        {
            ADOperationResult res = new ADOperationResult();


            if (memberDN != null && groupDN != null && requester != null)
            {
                var group = GetADObjectByDistinguishedName<Group>(groupDN);
                var member = GetADObjectByDistinguishedName<ADObjectBase>(memberDN);
                var user = FindUserByLogonName(requester);
                if (ValidateSecurity(group, user.DistinguishedName))
                {
                    try
                    {
                        if (member.DistinguishedName == group.DistinguishedName)
                        {

                            res.ErrorMessage = "A group cannot be member of itself";
                            return res;
                        }
                        DirectoryEntry dirEntry = GetDE(group.ADSPath);
                        dirEntry.Properties["member"].Add(member.DistinguishedName);
                        dirEntry.CommitChanges();
                        dirEntry.Close();
                        res.Success = true;
                        
                    }
                    catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
                    {
                        res.ErrorMessage = Ex.Message;

                    }
                    catch(Exception ex)
                    {
                        res.ErrorMessage = ex.Message;
                    }
                    return res;
                }
                res.ErrorMessage = " You do not have permission to modify members of this group";

            }
            else
            {

                res.ErrorMessage = "invalid parameters";
            }
            return res;

        }

        public ADOperationResult RemoveMemberFromGroup(string groupDN, string memberDN, string requester)
        {
            ADOperationResult res = new ADOperationResult();


            if (memberDN != null && groupDN != null && requester != null)
            {
                var group = GetADObjectByDistinguishedName<Group>(groupDN);
                var member = GetADObjectByDistinguishedName<ADObjectBase>(memberDN);
                var user = FindUserByLogonName(requester);
                if (ValidateSecurity(group, user.DistinguishedName))
                {
                    try
                    {
                        DirectoryEntry dirEntry = GetDE(group.ADSPath);
                        dirEntry.Properties["member"].Remove(member.DistinguishedName);
                        dirEntry.CommitChanges();
                        dirEntry.Close();
                        res.Success = true;

                    }
                    catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
                    {
                        res.ErrorMessage = Ex.Message;

                    }
                    catch (Exception ex)
                    {
                        res.ErrorMessage = ex.Message;
                    }
                    return res;
                }
                res.ErrorMessage = " You do not have permission to modify members of this group";

            }
            else
            {

                res.ErrorMessage = "invalid parameters";
            }
            return res;

        }


        public bool AddUserToGroup(string userEmail, string groupEmail)
        {

            var group = FindGroupBySMTPAddress(groupEmail);
            var user = FindADObjectBySMTPAddress(userEmail);
            if (user != null && group != null)
            {
                try
                {
                    if (user.DistinguishedName == group.DistinguishedName)
                        throw new InvalidOperationException("A group cannot be member of itself");
                    DirectoryEntry dirEntry = GetDE(group.ADSPath);
                    dirEntry.Properties["member"].Add(user.DistinguishedName);
                    dirEntry.CommitChanges();
                    dirEntry.Close();
                    return true;
                }
                catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
                {
                    //doSomething with E.Message.ToString();

                }
            }
            return false;

        }

        public bool RemoveUserFromGroup(string userEmail, string groupEmail)
        {
            var group = FindGroupBySMTPAddress(groupEmail);
            var user = FindADObjectBySMTPAddress(userEmail);
            if (user != null && group != null)
            {
                try
                {
                    DirectoryEntry dirEntry = GetDE(group.ADSPath);
                    dirEntry.Properties["member"].Remove(user.DistinguishedName);
                    dirEntry.CommitChanges();
                    dirEntry.Close();
                    return true;
                }
                catch (System.DirectoryServices.DirectoryServicesCOMException Ex)
                {
                    //doSomething with E.Message.ToString();

                }
            }
            return false;

        }


        public ADSearchResult ANRSearch(string searchString, int pagesize = 20)
        {

            if (searchString.IsNullOrEmpty()) throw new ArgumentNullException("searchFiler");

            string ldap = string.Format("(&(anr={0})(|(objectCategory=person)(objectCategory=group)))", EscapeLdap(searchString));
            // Specify AdProperties as the properties to retrieve 
            // from Active Directory during the search.
            Ds.PropertiesToLoad.Clear();
            ds.PageSize = pagesize;
            Ds.PropertiesToLoad.AddRange(AdProperties); Ds.PropertiesToLoad.AddRange(AdProperties);

            // Set the LDAP format filter string, since we're using LDAP as the
            // service provider for Active Directory Domain Services.
            Ds.Filter = ldap;

            // Execute search in Active Directory and 
            // get the first SearchResult object.
            SearchResultCollection results = Ds.FindAll(); ;

            var objects = new ADSearchResult(results, pagesize);
            return objects;
        }
        protected Person FindPersonByDistinguishedName(string distinguishedName)
        {
            Person person = null;

            if (!string.IsNullOrWhiteSpace(distinguishedName))
            {
                // Specify AdProperties as the properties to retrieve 
                // from Active Directory during the search.
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);

                // Set the LDAP format filter string, since we're using LDAP as the
                // service provider for Active Directory Domain Services.
                Ds.Filter =
                    "(&(distinguishedname=" + EscapeLdap(distinguishedName) + ")(objectcategory=Person))";

                // Execute search in Active Directory and 
                // get the first SearchResult object.
                SearchResult result = Ds.FindOne();

                // Create and initialize Person object from the 
                // SearchResult object.
                person = PersonFromSearchResult(result);
            }

            return person;
        }

        public T GetADObjectByDistinguishedName<T>(string distinguishedName) where T : ADObjectBase
        {
            string ldap = "(distinguishedName=" + distinguishedName + ")";
            Ds.PropertiesToLoad.Clear();
            Ds.PropertiesToLoad.AddRange(AdProperties);

            // Set the LDAP format filter string, since we're using LDAP as the
            // service provider for Active Directory Domain Services.
            Ds.Filter = ldap;

            // Execute search in Active Directory and 
            // get the first SearchResult object.
            SearchResult result = Ds.FindOne(); ;
            var _result = result.ToADObjectType();
            if (_result is T)
            {
                return (T)_result;
            }
            return null;
        }


        public Group FindGroupBySMTPAddress(string SMTPAddress, bool includeMember = false, int pageSize = 0)
        {
            Group group = null;

            if (!string.IsNullOrWhiteSpace(SMTPAddress))
            {
                // Specify AdProperties as the properties to retrieve 
                // from Active Directory during the search.
                Ds.PropertiesToLoad.Clear();
                ds.PageSize = pageSize;
                Ds.PropertiesToLoad.AddRange(AdProperties);

                // Set the LDAP format filter string, since we're using LDAP as the
                // service provider for Active Directory Domain Services.
                Ds.Filter = "(&(|(proxyaddresses=smtp:" + SMTPAddress + ")(mail=" + SMTPAddress + "))(objectcategory=group))";


                // Execute search in Active Directory and
                // get the first SearchResult object.
                SearchResult result = Ds.FindOne();

                // Create and initialize Person object from the 
                // SearchResult object.
                group = GroupFromSearchResult(result);

                if (includeMember)
                {
                    DirectorySearcher memSearcher = new DirectorySearcher(GetDE(result.Path));
                    memSearcher.AttributeScopeQuery = "member";
                    memSearcher.PropertiesToLoad.AddRange(ADSearcher.AdProperties);
                    SearchResultCollection memresults = memSearcher.FindAll();
                    foreach (SearchResult memitem in memresults)
                    {
                        group.Members.Add(memitem.ToADObjectType());
                    }
                }

            }

            return group;

        }

        public Group FindGroupByAttribute(string searchString, string attributeName, bool includeMember = false)
        {
            Group group = null;

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                // Specify AdProperties as the properties to retrieve 
                // from Active Directory during the search.
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);

                // Set the LDAP format filter string, since we're using LDAP as the
                // service provider for Active Directory Domain Services.
                Ds.Filter = "(&(" + attributeName + "=" + searchString + ")(objectcategory=group))";


                // Execute search in Active Directory and
                // get the first SearchResult object.
                SearchResult result = Ds.FindOne();

                // Create and initialize Person object from the 
                // SearchResult object.
                group = GroupFromSearchResult(result);

                if (includeMember)
                {
                    DirectorySearcher memSearcher = new DirectorySearcher(GetDE(result.Path));
                    memSearcher.AttributeScopeQuery = "member";
                    memSearcher.PropertiesToLoad.AddRange(ADSearcher.AdProperties);
                    SearchResultCollection memresults = memSearcher.FindAll();
                    foreach (SearchResult memitem in memresults)
                    {
                        group.Members.Add(memitem.ToADObjectType());
                    }
                }

            }

            return group;

        }



        public ADSearchResult SearchGroup(string searchString, bool includeMember = false, int pageSize = 0)
        {
            if (searchString.IsNullOrEmpty()) throw new ArgumentNullException("searchFiler");

            string ldap = string.Format("(&(anr={0})(objectCategory=group))", EscapeLdap(searchString));
            // Specify AdProperties as the properties to retrieve 
            // from Active Directory during the search.
            Ds.PropertiesToLoad.Clear();
            ds.PageSize = pageSize;
            Ds.PropertiesToLoad.AddRange(AdProperties);

            // Set the LDAP format filter string, since we're using LDAP as the
            // service provider for Active Directory Domain Services.
            Ds.Filter = ldap;

            // Execute search in Active Directory and 
            // get the first SearchResult object.
            SearchResultCollection results = Ds.FindAll(); ;

            var objects = new ADSearchResult(results, pageSize);
            if (includeMember)
            {
                foreach (var group in objects.Results.OfType<Group>())
                {
                    using (DirectorySearcher memSearcher = new DirectorySearcher(GetDE(group.ADSPath)))
                    {
                        memSearcher.AttributeScopeQuery = "member";
                        memSearcher.PropertiesToLoad.AddRange(ADSearcher.AdProperties);
                        SearchResultCollection memresults = memSearcher.FindAll();
                        foreach (SearchResult memitem in memresults)
                        {
                            group.Members.Add(memitem.ToADObjectType());
                        }
                    }
                }

            }
            return objects;

        }
        public ADSearchResult MyGroups(string userName, bool includeMember = false, int pageSize = 0)
        {
            return MyGroups(userName, null, includeMember, pageSize);

            if (userName.IsNullOrEmpty()) throw new ArgumentNullException("searchFiler");

            string ldap = string.Format("(&(objectCategory=User)(samAccountName={0}))", EscapeLdap(userName));
            // Specify AdProperties as the properties to retrieve 
            // from Active Directory during the search.
            Ds.PropertiesToLoad.Clear();
            ds.PageSize = pageSize;
            Ds.PropertiesToLoad.AddRange(new string[] { "distinguishedName" });

            // Set the LDAP format filter string, since we're using LDAP as the
            // service provider for Active Directory Domain Services.
            Ds.Filter = ldap;

            // Execute search in Active Directory and 
            // get the first SearchResult object.
            ADObjectBase user = Ds.FindOne().ToADObjectBaseInstance();

            var groupFilter = "(&(objectCategory=group)(|(managedBy={0})(msExchCoManagedByLink={0})))".xFormat(user.DistinguishedName);
            if (!user.DistinguishedName.IsNullOrEmpty())
            {
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);
                ds.PropertiesToLoad.Add("msExchCoManagedByLink");
                ds.Filter = groupFilter;

                SearchResultCollection groups = ds.FindAll();

                return new ADSearchResult(groups, pageSize);

            }
            return new ADSearchResult() { ErrorMessage = "User not found" };

        }
        public ADSearchResult MyGroups(string userName, string filter, bool includeMember = false, int pageSize = 0)
        {
            if (userName.IsNullOrEmpty()) throw new ArgumentNullException("searchFiler");

            string ldap = string.Format("(&(objectCategory=User)(samAccountName={0}))", EscapeLdap(userName));
            // Specify AdProperties as the properties to retrieve 
            // from Active Directory during the search.
            Ds.PropertiesToLoad.Clear();
            Ds.PageSize = pageSize;
            Ds.PropertiesToLoad.AddRange(new string[] { "distinguishedName" });

            // Set the LDAP format filter string, since we're using LDAP as the
            // service provider for Active Directory Domain Services.
            Ds.Filter = ldap;

            // Execute search in Active Directory and 
            // get the first SearchResult object.

            ADObjectBase user = Ds.FindOne().ToADObjectBaseInstance();
            string groupFilter = "";
            if (filter == null)
            {
                groupFilter = "(&(objectCategory=group)(|(managedBy={0})(msExchCoManagedByLink={0})))".xFormat(user.DistinguishedName);
            }
            else {
                groupFilter = "(&(objectCategory=group)(|(managedBy={0})(msExchCoManagedByLink={0}))(anr={1}))".xFormat(user.DistinguishedName, filter);
            }

            if (!user.DistinguishedName.IsNullOrEmpty())
            {
                Ds.PropertiesToLoad.Clear();
                Ds.PropertiesToLoad.AddRange(AdProperties);
                ds.PropertiesToLoad.Add("msExchCoManagedByLink");
                ds.Filter = groupFilter;

                SearchResultCollection groups = ds.FindAll();

                return new ADSearchResult(groups, pageSize);

            }
            return new ADSearchResult() { ErrorMessage = "User not found" };

        }

        public ADShortDetailResult GetDetailsForDNs(List<string> distinguishedNames, List<string> props)
        {
            LDAPFilterBuilder lb = new LDAPFilterBuilder();
            foreach (string dn in distinguishedNames)
            {
                lb.Or(PropertyNameConstants.distinguishedName, dn);
            }
            Ds.PropertiesToLoad.Clear();
            Ds.PropertiesToLoad.AddRange(props.ToArray());
            if (!props.Contains(PropertyNameConstants.distinguishedName))
            {
                ds.PropertiesToLoad.Add(PropertyNameConstants.distinguishedName);
            }
            Ds.Filter = lb.Build();
            SearchResultCollection results = Ds.FindAll();

            return new ADShortDetailResult(results, props);

        }

        internal DirectoryEntry GetDE(string path)
        {
            if (ADSConfig.Instance.UseCredential)
            {
                return new DirectoryEntry(path, ADSConfig.Instance.UserName, ADSConfig.Instance.Password);
            }
            return new DirectoryEntry(path);
        }

        private bool ValidateSecurity(Group group, string requesterDN)
        {
            if (group.OwnerDN != null && group.OwnerDN.Equals(requesterDN, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return (group.ExchangeCoOwnerDNs != null && group.ExchangeCoOwnerDNs.Contains(requesterDN, StringComparer.OrdinalIgnoreCase));
        }

    }
}
