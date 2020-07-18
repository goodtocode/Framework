using Microsoft.Extensions.Configuration;
using System;

namespace GoodToCode.Framework.Test
{
    public class AppSettingFactory
    {
        private IConfiguration _config;

        public string MyWebService { get { return _config["AppSettings:MyWebService"]; } }
        
        public AppSettingFactory()
        {
            _config = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT ") ?? "Development")}.json")
              .AddJsonFile("appsettings.json")
              .Build();
        }

        public AppSettingFactory(IConfiguration config)
        {
            _config = config;
        }

        public string GetMyWebService()
        {
            return MyWebService;
        }
    }
}
