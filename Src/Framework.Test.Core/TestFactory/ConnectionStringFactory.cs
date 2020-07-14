using Microsoft.Extensions.Configuration;
using System;

namespace GoodToCode.Framework.Test
{
    public class ConnectionStringFactory
    {
        private IConfiguration _config;

        public string DefaultConnection { get { return _config["ConnectionStrings:DefaultConnection"]; } }
        
        public ConnectionStringFactory()
        {
            _config = new ConfigurationBuilder()
              .AddJsonFile($"appsettings.{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT ") ?? "Development")}.json")
              .AddJsonFile("appsettings.json")
              .Build();
        }

        public ConnectionStringFactory(IConfiguration config)
        {
            _config = config;
        }

        public string GetDefaultConnection()
        {
            return DefaultConnection;
        }
    }
}
