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
using Framework.WebService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Framework.WebServices
{
    /// <summary>
    /// Default controller for .UseMvc
    /// </summary>
    public class HomeController : Controller
    {
        public const string ControllerName = "Home";
        public const string ContactUsView = "~/Views/Genesys-Source/Genesys-Contact.cshtml";
        public const string IndexGetView = "~/Views/Home/Index.cshtml";
        public const string IndexGetAction = "Index";
        public const string IndexPostAction = "PostIndex";
        public const string IndexPutAction = "PutIndex";
        public const string IndexDeleteAction = "DeleteIndex";

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
        public ActionResult PostIndex()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpPut route
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        public ActionResult PutIndex()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Default HttpDelete route
        /// </summary>
        /// <returns></returns>
        [HttpDelete()]
        public ActionResult DeleteIndex()
        {
            return View(HomeController.IndexGetView);
        }

        /// <summary>
        /// Error Page
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
           return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
