//-----------------------------------------------------------------------
// <copyright file="HomeApiController.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
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
using System;
using System.Web.Http;
using Genesys.Extras.Web.Http;

namespace Framework.WebServices
{
    /// <summary>
    /// Default WebApi controller
    /// </summary>
    public class HomeApiController : WebApiController
    {
        public const string ControllerName = "HomeApi";
        public const string IndexGetAction = "Index";
        public const string IndexPutAction = "IndexPut";
        public const string IndexPostAction = "IndexPost";
        public const string IndexDeleteAction = "IndexDelete";

        /// <summary>
        /// Default HttpGet route
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public string Index()
        {
            return String.Format("{0}{1}", WebApiController.MessageUpAndRunning, "Index() Get Method. Parameterless.");
        }

        /// <summary>
        /// Default HttpGet route with parameter
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public string Index(string id)
        {
            return String.Format("{0}{1}{2}", WebApiController.MessageUpAndRunning, "Index(string id) Get Method. id: ", id ?? "Null");
        }

        /// <summary>
        /// Default HttpPost route
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public string IndexPost()
        {
            return WebApiController.MessageUpAndRunning;
        }

        /// <summary>
        /// Default HttpPut route
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public string IndexPut()
        {
            return WebApiController.MessageUpAndRunning;
        }

        /// <summary>
        /// Default HttpDelete route
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public string IndexDelete()
        {
            return WebApiController.MessageUpAndRunning;
        }
    }
}
