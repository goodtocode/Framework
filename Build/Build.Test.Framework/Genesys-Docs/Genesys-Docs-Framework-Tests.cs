//-----------------------------------------------------------------------
// <copyright file="Genesys_Docs_Framework_Tests.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.s
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Extras.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Genesys.Build.Test 
{
    [TestClass()]
    public class Genesys_Docs_Framework_Tests
    {
        [TestMethod()]
        public async Task Genesys_Docs_Framework_QuickGuide_Pdfs()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkQuickGuide")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };

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
        public async Task Genesys_Docs_Framework_QuickGuide_Images()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkQuickGuide")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };

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
        public async Task Genesys_Docs_Framework_Requirements()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysFrameworkRequirements")) { CompletionOption = System.Net.Http.HttpCompletionOption.ResponseHeadersRead };
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