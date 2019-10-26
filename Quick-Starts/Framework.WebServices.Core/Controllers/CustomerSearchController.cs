//-----------------------------------------------------------------------
// <copyright file="CustomerSearchController.cs" company="GoodToCode">
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
using Framework.Customer;
using GoodToCode.Extensions;
using GoodToCode.Framework.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Framework.WebServices
{
    /// <summary>
    /// Searches for customer records    
    /// </summary>
    public class CustomerSearchController : Controller
    {
        public const string ControllerName = "CustomerSearch";
        public const string ControllerRoute = "v4/CustomerSearch";
        public const string GetActionText = "Search";
        public const string GetAction = "Get";
        public const string PostAction = "Post";

        /// <summary>
        /// Parameterized HttpGet search, refreshing only the results region
        ///  Path: /CustomerSearch?id={id}&firstName={firstName}&lastName={lastName}
        ///  Parameters are strings in order to validate, log and handle incorrect values
        /// </summary>
        /// <param name="id">Int32 - ID to include in search results</param>
        /// <param name="firstName">String - Text to search in first name</param>
        /// <param name="lastName">String - Text to search in the last name field</param>
        /// <returns>Partial view of only the search results region</returns>
        [HttpGet(ControllerRoute + "/{key}/{firstName}/{lastName}"), Route(ControllerRoute)]
        public IActionResult Get(string key = "-1", string firstName = "", string lastName = "")
        {
            var model = new CustomerSearchModel() { Id = key.TryParseInt32(), Key = key.TryParseGuid(), FirstName = firstName, LastName = lastName };
            var reader = new EntityReader<CustomerInfo>();
            var searchResults = reader.GetByWhere(x => x.Key == model.Key || x.FirstName.Contains(model.FirstName) || x.LastName.Contains(model.LastName) || x.BirthDate == model.BirthDate);


            if (searchResults.Any())
                model.Results.FillRange(searchResults);

            return Ok(model);
        }

        /// <summary>
        /// Performs a full HttpPost search, accepting search parameters and returning search parameters and results
        /// </summary>
        /// <param name="data">Model of type ICustomer with results list</param>
        /// <returns>JSON of search parameters and any found results</returns>
        [HttpPost(ControllerRoute)]
        public IActionResult Post([FromBody]CustomerModel data)
        {
            var model = new CustomerSearchModel();
            var reader = new EntityReader<CustomerInfo>();
            var searchResults = reader.GetByWhere(x => x.Key == data.Key || x.FirstName.Contains(data.FirstName) || x.LastName.Contains(data.LastName) || x.BirthDate == data.BirthDate);
            var form = Request.ReadFormAsync();
            model.Fill(data);
            if (searchResults.Any())
                model.Results.FillRange(searchResults);

            return Ok(model);
        }     
    }
}