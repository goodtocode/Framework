//-----------------------------------------------------------------------
// <copyright file="HttpRequestBaseTests.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Extras.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Genesys.Build.Test
{
    [TestClass()]
    public class HttpRequestGetTests
    {
        [TestMethod()]
        public async Task Net_HttpRequestGetString_SendAsync()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;            
            var request = new HttpRequestGetString(configuration.AppSettingValue(AppSettingList.MyWebServiceKeyName) + "/HomeApi");
            dataOut = await request.SendAsync();
            Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
        }

        [TestMethod()]
        public async Task Net_HttpRequestGet_SendAsync()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            object dataOut;
            var request = new HttpRequestGet<object>(configuration.AppSettingValue(AppSettingList.MyWebServiceKeyName) + "/HomeApi");
            dataOut = await request.SendAsync();
            Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
        }
    }
}