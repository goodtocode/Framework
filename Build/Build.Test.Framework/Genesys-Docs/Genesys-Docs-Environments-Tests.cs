//-----------------------------------------------------------------------
// <copyright file="Genesys_Docs_Environments_Tests.cs" company="Genesys Source">
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
    public class Genesys_Docs_Environments_Tests
    {
        [TestMethod()]
        public async Task Genesys_Docs_Environments_Mvc()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforMvcZip"));
            //try/catch until following feature is released in 2017.07: request.CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead;
            try
            {
                dataOut = await request.SendAsync();
                Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
            }
            catch(System.Net.Http.HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("Cannot write more bytes to the buffer than the configured maximum buffer size"));
            }                       
        }

        [TestMethod()]
        public async Task Genesys_Docs_Environments_WebApi()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforWebApiZip"));
            //try/catch until following feature is released in 2017.07: request.CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead;
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
        public async Task Genesys_Docs_Environments_Wpf()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforWpfZip"));
            //try/catch until following feature is released in 2017.07: request.CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead;
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
        public async Task Genesys_Docs_Environments_Universal()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkforUniversalZip"));
            //try/catch until following feature is released in 2017.07: request.CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead;
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