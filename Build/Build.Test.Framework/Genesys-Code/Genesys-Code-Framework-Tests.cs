//-----------------------------------------------------------------------
// <copyright file="Genesys_Code_Framework_Tests.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Extras.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Genesys.Build.Test
{
    [TestClass()]
    public class Genesys_Code_Framework_Tests
    {
        [TestMethod()]
        public async Task Genesys_Code_Framework_Mvc()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforMvcZip")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };

            try
            {
                dataOut = await request.SendAsync();
                Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot write more bytes to the buffer than the configured maximum buffer size"));
            }
        }

        [TestMethod()]
        public async Task Genesys_Code_Framework_WebApi()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforWebApiZip")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };
            try
            {
                dataOut = await request.SendAsync();
                Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot write more bytes to the buffer than the configured maximum buffer size"));
            }
        }

        [TestMethod()]
        public async Task Genesys_Code_Framework_Wpf()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforWpfZip")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };
            try
            {
                dataOut = await request.SendAsync();
                Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot write more bytes to the buffer than the configured maximum buffer size"));
            }
        }

        [TestMethod()]
        public async Task Genesys_Code_Framework_Universal()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforUniversalZip")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };
            try
            {
                dataOut = await request.SendAsync();
                Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot write more bytes to the buffer than the configured maximum buffer size"));
            }
        }
    }
}