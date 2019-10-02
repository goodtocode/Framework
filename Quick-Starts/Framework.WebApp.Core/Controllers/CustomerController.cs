//-----------------------------------------------------------------------
// <copyright file="CustomerController.cs" company="GoodToCode">
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
using GoodToCode.Extensions.Configuration;
using GoodToCode.Framework.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Data.SqlClient;

namespace Framework.WebApp
{
    /// <summary>
    /// Creates a Customer
    /// </summary>
    [Authorize]
    public class CustomerController : Controller
    {
        public const string ControllerName = "Customer";
        public const string SummaryAction = "Summary";
        public const string SummaryView = "~/Views/Customer/CustomerSummary.cshtml";
        public const string CreateAction = "Create";
        public const string CreateActionText = CreateAction;
        public const string CreateView = "~/Views/Customer/CustomerCreate.cshtml";
        public const string EditAction = "Edit";
        public const string EditView = "~/Views/Customer/CustomerEdit.cshtml";
        public const string DeleteAction = "Delete";
        public const string DeleteView = "~/Views/Customer/CustomerDelete.cshtml";
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
        /// Displays entity
        /// </summary>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Summary(string id)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = (id.IsInteger() ?
                reader.GetById(id.TryParseInt32()) :
                reader.GetByKey(id.TryParseGuid()));
            if (customer.IsNew)
                TempData[ResultMessage] = "Customer not found";

            return View(CustomerController.SummaryView, customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Customer Summary with Edit/Delete functionality
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult Summary(CustomerModel model)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetById(model.Id);
            return View(CustomerController.EditView, customer.CastOrFill<CustomerModel>());
        }
        /// <summary>
        /// Displays entity for editing
        /// </summary>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult Create()
        {
            return View(CustomerController.CreateView, new CustomerModel());
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model">Full customer model worth of data to be saved</param>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult Create(CustomerModel model)
        {
            var customer = model.CastOrFill<CustomerInfo>();

            customer = customer.Save();
            if (!customer.IsNew)
                TempData[ResultMessage] = "Successfully created";
            else
                TempData[ResultMessage] = "Failed to create";

            return View(CustomerController.SummaryView, customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Displays entity for editing
        /// </summary>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult Edit(string id)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetById(id.TryParseInt32());

            if (customer.IsNew)
                TempData[ResultMessage] = "No customer found";

            return View(CustomerController.EditView, customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model">Full customer model worth of data with user changes</param>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult Edit(CustomerModel model)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = model.CastOrFill<CustomerInfo>();

            customer = customer.Save();
            if (!customer.IsNew)
                TempData[ResultMessage] = "Successfully saved";
            else
                TempData[ResultMessage] = "Failed to save";

            return View(CustomerController.SummaryView, customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Displays entity for deleting
        /// </summary>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult Delete(string id)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetById(id.TryParseInt32());

            if (customer.IsNew)
                TempData[ResultMessage] = "No customer found";                

            return View(CustomerController.DeleteView, customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Deletes a customer
        /// </summary>
        /// <param name="model">Customer to delete</param>
        /// <returns>View rendered with model data</returns>
        [AllowAnonymous]
        [HttpPost()]
        public ActionResult Delete(CustomerModel model)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetByKey(model.Key);

            customer = customer.Delete();
            if (customer.IsNew)
                TempData[ResultMessage] = "Successfully deleted";
            else
                TempData[ResultMessage] = "Failed to delete";

            return View(CustomerSearchController.SearchView, customer.CastOrFill<CustomerSearchModel>());
        }

        /// <summary>
        /// Can connect to the database?
        /// </summary>
        /// <returns></returns>
        public static bool CanConnect()
        {
            var returnValue = Defaults.Boolean;
            var configuration = new ConfigurationManagerCore(ApplicationTypes.Native);
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                returnValue = connection.CanOpen();
            }
            return returnValue;
        }
    }
}