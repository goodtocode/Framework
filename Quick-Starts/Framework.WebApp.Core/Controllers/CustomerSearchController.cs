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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Framework.WebApp
{
    /// <summary>
    /// Creates a Customer
    /// </summary>
    [Authorize]
    public class CustomerSearchController : Controller
    {
        public const string ControllerName = "CustomerSearch";
        public const string SearchAction = "Search";
        public const string SearchActionText = SearchAction;
        public const string SearchView = "~/Views/CustomerSearch/CustomerSearch.cshtml";
        public const string SearchResultsAction = "SearchResults";
        public const string SearchResultsView = "~/Views/CustomerSearch/CustomerSearchResults.cshtml";
        public const string ResultMessage = "ResultMessage";

        /// <summary>
        /// Called right before action methods executed
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            TempData[ResultMessage] = Defaults.String;
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// Shows the search page
        /// </summary>
        /// <returns>Initialized search page</returns>
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult Search()
        {
            return View(CustomerSearchController.SearchView, new CustomerSearchModel());
        }

        /// <summary>
        /// Performs a full-post back search, accepting search parameters and returning search parameters and results
        /// </summary>
        /// <param name="model">Model of type ICustomer with results</param>
        /// <returns>View of search parameters and any found results</returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult Search(CustomerModel data)
        {
            var model = new CustomerSearchModel();
            IQueryable<CustomerInfo> searchResults;

            model = data.CastOrFill<CustomerSearchModel>();
            using (var reader = new EntityReader<CustomerInfo>())
            {
                searchResults = reader.GetAll()
                    .Where(x => (model.FirstName != Defaults.String && x.FirstName.Contains(model.FirstName))
                        || (model.LastName != Defaults.String && x.LastName.Contains(model.LastName))
                        || (model.BirthDate != Defaults.Date && x.BirthDate == model.BirthDate)
                        || (x.Id == model.Id)).Take(25);
            }
            if (searchResults.Any())
                model.Results.FillRange(searchResults);
            TempData[ResultMessage] = $"{model.Results.Count} matches found";

            return View(CustomerSearchController.SearchView, model);
        }

        /// <summary>
        /// Client-side version of search, refreshing only the results region
        /// </summary>
        /// <param name="id">ID to include in search results</param>
        /// <param name="firstName">Text to search in first name</param>
        /// <param name="lastName">Text to search in the last name field</param>
        /// <returns>Partial view of only the search results region</returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult SearchResults(string id, string firstName, string lastName)
        {
            var model = new CustomerSearchModel() { Id = id.TryParseInt32(), Key = id.TryParseGuid(), FirstName = firstName, LastName = lastName };
            IQueryable<CustomerInfo> searchResults;

            using (var reader = new EntityReader<CustomerInfo>())
            {
                searchResults = reader.GetAll()
                    .Where(x => (model.FirstName != Defaults.String && x.FirstName.Contains(model.FirstName))
                        || (model.LastName != Defaults.String && x.LastName.Contains(model.LastName))
                        || (model.BirthDate != Defaults.Date && x.BirthDate == model.BirthDate)
                        || (x.Id == model.Id)).Take(25);
            }
            if (searchResults.Any())
                model.Results.FillRange(searchResults);
            TempData[ResultMessage] = $"{model.Results.Count} matches found";

            return PartialView(CustomerSearchController.SearchResultsView, model.Results);
        }
    }
}