﻿//-----------------------------------------------------------------------
// <copyright file="Sampler_Framework_for_MVC.cs" company="Genesys Source">
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Genesys.Extensions;
using Genesys.Extras.Net;
using Genesys.Extras.Configuration;
using System.Threading.Tasks;

namespace Framework.Tests
{
    /// <summary>
    /// Test framework functionality
    /// </summary>
    /// <remarks></remarks>
    [TestClass()]
    public class Sampler_Framework_for_WebApi
    {
        /// <summary>
        /// Get a customer from the cloud
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Endpoints_Framework_WebAPI_CustomerSearchGet()
        {
            var url = new ConfigurationManagerFull().AppSettingValue("MyWebService");
            var request = new HttpRequestGetString(url + "/CustomerSearch/-1/i/x/");
            var returnValue = await request.SendAsync();
            Assert.IsTrue(request.Response.IsSuccessStatusCode, request.Response.ReasonPhrase);
            Assert.IsTrue(returnValue != TypeExtension.DefaultString);
        }
    }
}