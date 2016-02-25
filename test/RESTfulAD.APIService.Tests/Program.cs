using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RESTfulAD.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Threading.Tasks;

namespace RESTfulAD.APIService.Tests
{
    public class Program
    {

        public IConfigurationRoot Configuration { get; set; }
        public Program()
        {
            //// Set up configuration sources.
            //var builder = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables();
            //Configuration = builder.Build();

            //ADSConfig.Instance = Configuration.Get<ADSConfig>();


        }
        public static void Main(string[] args)
        {


            SearchUtility su = new SearchUtility();

            WriteObjectAsJson(su.FindGroupBySMTPAddress("abc@mysupport.in"));

            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
            WriteObjectAsJson(su.FindGroupBySMTPAddress("abcsec@mysupport.in", true));
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");

            return;









            //Console.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ADSConfig.Instance));
            DirectoryContext context = new DirectoryContext(DirectoryContextType.DirectoryServer, RESTfulAD.APIService.Tests.ADSConfig.Instance.Server, RESTfulAD.APIService.Tests.ADSConfig.Instance.UserName, RESTfulAD.APIService.Tests.ADSConfig.Instance.Password);
            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.56.1/CN=abc@mysupport.in,OU=ou1,DC=mysupport,DC=in", RESTfulAD.APIService.Tests.ADSConfig.Instance.UserName, RESTfulAD.APIService.Tests.ADSConfig.Instance.Password);
            //ADObjectBase adb = new ADObjectBase(de);
            //return;
            DirectorySearcher ds = new DirectorySearcher(de);
            //ds.SearchScope = SearchScope.Base;
            ds.AttributeScopeQuery = "member";
            ds.PropertiesToLoad.AddRange(ADSearcher.AdProperties);
            List<ADObjectBase> objlist = new List<ADObjectBase>();
            ds.Filter = "(mail=*)";
            foreach (SearchResult item in ds.FindAll())
            {
                objlist.Add(new ADObjectBase(item));
            }
            Console.Write(Newtonsoft.Json.JsonConvert.SerializeObject(objlist[0]));


            Console.WriteLine(objlist.Count);
            Console.ReadLine();

        }
        static void WriteObjectAsJson(object value)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(value));

        }
    }
}
