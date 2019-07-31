//-----------------------------------------------------------------------
// <copyright file="HomeApiController.cs" company="GoodToCode">
//      Copyright (c) 2017-2018 GoodToCode. All rights reserved.
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
using GoodToCode.Extras.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Framework.WebServices
{
    /// <summary>
    /// Default WebApi controller
    /// </summary>
    [Route(ControllerRoute)]
    public class HomeApiController : WebApiController
    {
        public const string ControllerName = "HomeApi";
        public const string ControllerRoute = "v4/HomeApi";
        public const string IndexGetAction = "Get";
        public const string IndexPutAction = "Put";
        public const string IndexPostAction = "Post";
        public const string IndexDeleteAction = "Delete";

        /// <summary>
        /// Default HttpGet route
        /// </summary>
        /// <returns></returns>
        [HttpGet()]        
        public string Get()
        {
            return WebApiController.MessageUpAndRunning;
        }

        /// <summary>
        /// Default HttpPost route
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public string Post()
        {
            return WebApiController.MessageUpAndRunning;
        }

        /// <summary>
        /// Default HttpPut route
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public string Put()
        {
            return WebApiController.MessageUpAndRunning;
        }

        /// <summary>
        /// Default HttpDelete route
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public string Delete()
        {
            return WebApiController.MessageUpAndRunning;
        }
    }
}
