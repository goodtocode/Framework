
using GoodToCode.Framework.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoodToCode.Framework.Test
{
    [TestClass]
    public class HttpQueryTests
    {
        private static IOptions<HttpQueryOptions> _options;

        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", false)
            //    .Build();
            // configuration.GetSection("HttpCrudEndpoints").Bind(optionsData);
            var optionsData = new HttpQueryOptions() { new HttpQueryOption() { Type = "CustomerDto", Url = "https://entities-for-webapi.azurewebsites.net/v4/PersonSearch" } };            
            _options = Options.Create<HttpQueryOptions>(optionsData);            
        }

        [TestMethod]
        public void HttpQueryOptions_Construction()
        {
            var service = new HttpQueryService<CustomerDto>(_options);
            Assert.IsTrue(!string.IsNullOrEmpty(service.TypeName));
            Assert.IsTrue(service.Uri != new Uri("http://localhost:80", UriKind.RelativeOrAbsolute));
        }
    }
}
