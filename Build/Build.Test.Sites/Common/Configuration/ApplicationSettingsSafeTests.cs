//-----------------------------------------------------------------------
// <copyright file="ConfigurationManagerSafeTests.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
//      All rights are reserved. Reproduction or transmission in whole or in part, in
//      any form or by any means, electronic, mechanical or otherwise, is prohibited
//      without the prior written consent of the copyright owner.
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using System.Collections.Specialized;

namespace Genesys.Build.Test
{
    /// <summary>
    /// ConfigurationManagerSafe Tests
    /// </summary>
    [TestClass()]
    public partial class ApplicationSettingSafeTests
    {
        /// <summary>
        /// Connection strings in safe version of configuration manager
        /// </summary>
        [TestMethod()]
        public void Configuration_ApplicationSettingSafe()
        {
            var itemToTest = new AppSettingSafe();
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();

            itemToTest = configuration.AppSetting("TestAppSetting");
            Assert.IsTrue(itemToTest.Value != TypeExtension.DefaultString);
        }

        /// <summary>
        /// Universal cant access ConfigurationManager directly. 
        ///  This method uses the ConfigurationManager to get data, then returns as a cross-platform friendly array
        /// </summary>
        /// <returns></returns>
        public static string[,] AppSettingsGet()
        {
            var itemToConvert = ConfigurationManager.AppSettings ?? new NameValueCollection();
            string[,] returnValue = new string[itemToConvert.Count, 2];

            for (var count = 0; count < itemToConvert.Count; count++)
            {
                returnValue[count, 0] = itemToConvert.Keys[count];
                returnValue[count, 1] = itemToConvert[count];
            }

            return returnValue;
        }
    }
}