using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace RESTfulAD.APIService.Tests
{
    public class ExtentionMethodTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public ExtentionMethodTests(ITestOutputHelper helper)
        {
            _outputHelper = helper;
        }

        [Theory]
        [InlineData("CN=DnsUpdateProxy,CN=Users,DC=contoso,DC=com", "contoso.com")]
        [InlineData("DC=contoso,DC=com", "contoso.com")]
        [InlineData("DC=contoso,D=com", null)]
        public void ConvertDNtoFQDN(string value, string expected)
        {
            Assert.Equal(expected, ExtensionMethods.GetDomainFQDNfromDN(value));
        }

        [Fact]
        public void TestConfig()
        {
            Trace.TraceInformation(Newtonsoft.Json.JsonConvert.SerializeObject(ADSConfig.Instance));
            Assert.IsType<ADSConfig>(ADSConfig.Instance);
            
        }
        
    }
}
