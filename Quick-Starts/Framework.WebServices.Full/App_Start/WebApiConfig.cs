//-----------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="Genesys Source">
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
using System.Web.Http;

namespace Framework.WebServices
{
    /// <summary>
    /// WebApiConfig
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="config">config</param>6
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            // Route for WebApi's default /api/Controller. For familiarity with standard Web API.e
            config.Routes.MapHttpRoute(
                name: "DefaulApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // Route for /v4/Controller. Allows expansion through versioning /v4/.
            // Important: Default parameter "id" (above) changed to "key" (below)
            config.Routes.MapHttpRoute(
                name: "DefaultV1",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            
            // Route for /v4/ (with no controller). Used to test is services is listening to any requests.
            config.Routes.MapHttpRoute(
                name: "DefaultV1Naked",
                routeTemplate: "v1",
                defaults: new { controller = HomeApiController.ControllerName, action = HomeApiController.IndexGetAction });
        }
    }
}
