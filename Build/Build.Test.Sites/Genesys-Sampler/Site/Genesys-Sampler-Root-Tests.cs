//-----------------------------------------------------------------------
// <copyright file="_Genesys_Sampler_Root_Tests.cs" company="Genesys Source">
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
    public class Genesys_Sampler_Root_Tests
    {
        [TestMethod()]
        public async Task Genesys_Sampler_Root()
        {
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();
            var dataOut = TypeExtension.DefaultString;            
            var request = new HttpRequestGetString(configuration.AppSettingValue("GenesysSamplerRoot"));
            dataOut = await request.SendAsync();
            Assert.IsTrue(request.Response.IsSuccessStatusCode == true);
        }
    }
}