using Microsoft.Extensions.Configuration;
using RESTfulAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.APIService.Tests
{
    public class ADSConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Domain { get; set; }

        public string RootDSE { get { return string.Format("LDAP://{0}/rootDSE", Server); } }
        public string DomainDSE { get { return string.Format("LDAP://{0}/{1}", Server, Domain.GetDomainDNfromFQDN()); } }

        private static ADSConfig instance;
        internal static ADSConfig Instance
        {
            get
            {
                if(instance == null)
                {
                    init();
                }
                return instance;
            }
        }

        public static void init()
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("credential.json")
                .AddEnvironmentVariables();
            var Configuration = builder.Build();

            instance = Configuration.Get<ADSConfig>("ADSConfig");
        }

    }
}
