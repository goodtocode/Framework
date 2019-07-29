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
    public partial class ConnectionStringSafeTests
    {
        /// <summary>
        /// Read connection information for embedded databases (on devices, primarily)
        /// </summary>
        [TestMethod()]
        public void Configuration_ConnectionStringSafe()
        {
            var itemToTest = new ConnectionStringSafe();
            var configuration = ConfigurationManagerSafeTests.ConfigurationManagerSafeConstruct();

            // ADO
            itemToTest = configuration.ConnectionString("TestADOConnection");
            Assert.IsTrue(itemToTest.Value != TypeExtension.DefaultString);
            itemToTest.EDMXFileName = "TestEDMXFile";
            Assert.IsTrue(itemToTest.ToString("EF") != TypeExtension.DefaultString);
            Assert.IsTrue(itemToTest.ToEF(this.GetType()) != TypeExtension.DefaultString);
            // EF
            itemToTest = configuration.ConnectionString("TestEFConnection");
            Assert.IsTrue(itemToTest.Value != TypeExtension.DefaultString);
            Assert.IsTrue(itemToTest.ToString("ADO") != TypeExtension.DefaultString);
            Assert.IsTrue(itemToTest.ToADO() != TypeExtension.DefaultString);
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
    }
}