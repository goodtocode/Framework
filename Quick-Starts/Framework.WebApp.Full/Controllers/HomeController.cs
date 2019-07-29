//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Genesys Source">
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
using System.Web.Mvc;
using Genesys.Extras.Web.Http;

namespace Framework.WebApp
{
    /// <summary>
    /// Default Mvc controller
    /// </summary>
    public class HomeController : MvcController
    {
        public const string ControllerName = "Home";
        public const string IndexGetView = "Index";
        public const string IndexGetAction = "Index";
        public const string IndexPostAction = "IndexPost";
        public const string IndexPutAction = "IndexPut";
        public const string IndexDeleteAction = "IndexDelete";
        public const string AboutUsView = "About";
        public const string ContactUsView = "Contact";

        /// <summary>
        /// Default HttpGet route
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Index()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpPost route
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult IndexPost()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpPut route
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public ActionResult IndexPut()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpDelete route
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public ActionResult IndexDelete()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// About Us view
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult About()
        {
            return View(HomeController.AboutUsView);
        }

        /// <summary>
        /// Contact Us view
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult Contact()
        {
            return View(HomeController.ContactUsView);
        }
    }
}
