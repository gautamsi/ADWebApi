using Microsoft.Extensions.Configuration;
using RESTfulAD.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.APIService
{
    public class ADSConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string DefaultDomain { get; set; }
        public bool UseCredential { get; set; }
        public string DefaultSearchRoot { get; set; }
        public string DefaultGlobalCatalog { get; set; }
        public bool UseServer { get; set; }
        public bool UseRootDSE { get; set; }

        public string RootDSE { get { return "LDAP://rootDSE"; } }
        public string RootDSEWithServer { get { return "LDAP://{0}/rootDSE".xFormat(Server); } }
        public string DomainDSE { get { return "LDAP://{0}".xFormat(DefaultDomain.GetDomainDNfromFQDN()); } }
        public string DomainDSEWithServer { get { return "LDAP://{0}/{1}".xFormat(Server, DefaultDomain.GetDomainDNfromFQDN()); } }

        public int MaxSearchResult { get; set; } = 10;

        private static ADSConfig instance;
        internal static ADSConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    init();
                }
                return instance;
            }
        }

        public static void init()
        {
            
            
            instance = Startup.Configuration.Get<ADSConfig>("ADSConfig");
        }

        public DirectorySearcher GetSearcher()
        {
            DirectoryEntry de;
            string path = string.Empty;
            if (UseRootDSE)
            {
                path = UseServer? RootDSEWithServer: RootDSE;
            }
            if (!DefaultGlobalCatalog.IsNullOrEmpty())
            {
                path = DefaultGlobalCatalog;
            }
            if (!DefaultSearchRoot.IsNullOrEmpty())
            {
                path = DefaultSearchRoot;
            }
            if (path.IsNullOrEmpty())
            {
                path = UseServer ? RootDSEWithServer : RootDSE;
            }
            if (UseCredential)
            {
                de = new DirectoryEntry(path, UserName, Password);
                return new DirectorySearcher(de);
            }
            else
            {
                return new DirectorySearcher(DefaultSearchRoot ?? path);

            }            

        }

    }
}
