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
    public partial class ConfigurationManagerSafeTests
    {
        /// <summary>
        /// Connection strings in safe version of configuration manager
        /// </summary>
        [TestMethod()]
        public void Configuration_ConfigurationManagerSafe_AppSettings()
        {
            var itemToTest = new AppSettingSafe();
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();

            itemToTest = configuration.AppSetting("TestAppSetting");
            Assert.IsTrue(itemToTest.Value != TypeExtension.DefaultString);
        }

        /// <summary>
        /// Read connection information for embedded databases (on devices, primarily)
        /// </summary>
        [TestMethod()]
        public void Configuration_ConfigurationManagerSafe_ConnectionStrings()
        {
            var itemToTest = new ConnectionStringSafe();
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();

            // Validity
            itemToTest.Value = "Invalid Connection String!!!";
            Assert.IsTrue(itemToTest.IsValid == false);
            // ADO
            itemToTest = configuration.ConnectionString("TestADOConnection");
            Assert.IsTrue(itemToTest.ToString("EF") != TypeExtension.DefaultString);
            Assert.IsTrue(itemToTest.IsADO == true);
            Assert.IsTrue(itemToTest.IsEF == false);
            Assert.IsTrue(itemToTest.IsValid == true);
            Assert.IsTrue(itemToTest.ConnectionStringType == ConnectionStringSafe.ConnectionStringTypes.ADO);
            Assert.IsTrue(itemToTest.ToEF(this.GetType()) != TypeExtension.DefaultString);
            // EF
            itemToTest = configuration.ConnectionString("TestEFConnection");
            Assert.IsTrue(itemToTest.ToString("ADO") != TypeExtension.DefaultString);
            Assert.IsTrue(itemToTest.IsEF == true);
            Assert.IsTrue(itemToTest.IsADO == false);
            Assert.IsTrue(itemToTest.IsValid == true);
            Assert.IsTrue(itemToTest.ConnectionStringType == ConnectionStringSafe.ConnectionStringTypes.EF);
            Assert.IsTrue(itemToTest.ToEF(this.GetType()) != TypeExtension.DefaultString);
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

        /// <summary>
        /// Universal cant access ConfigurationManager directly. 
        ///  This method uses the ConfigurationManager to get data, then returns as a cross-platform friendly array
        /// </summary>
        /// <returns></returns>
        public static string[,] ConnectionStringsGet()
        {
            var itemToConvert = ConfigurationManager.ConnectionStrings ?? new ConnectionStringSettingsCollection();
            string[,] returnValue = new string[itemToConvert.Count, 2];

            for (var count = 0; count < itemToConvert.Count; count++)
            {
                returnValue[count, 0] = itemToConvert[count].Name;
                returnValue[count, 1] = itemToConvert[count].ConnectionString;
            }

            return returnValue;
        }

        /// <summary>
        /// Constructs a current instance of .config AppSettings and ConnectionStrings nodes
        /// Universal/Core does not support ConfigurationManager, so have to construct using Universal friendly means
        /// </summary>
        /// <returns></returns>
        public static ConfigurationManagerSafe ConfigurationManagerSafeConstruct()
        {
            return new ConfigurationManagerSafe(ConfigurationManagerSafeTests.AppSettingsGet(), ConfigurationManagerSafeTests.ConnectionStringsGet());
        }

    }
}