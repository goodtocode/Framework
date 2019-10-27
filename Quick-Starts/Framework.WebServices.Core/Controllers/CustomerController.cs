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
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Framework.WebServices
{
    /// <summary>
    /// Accepts HttpGet, HttpPut, HttpPost and HttpDelete operations on a customer
    /// </summary>
    public class CustomerController : Controller
    {
        public const string ControllerName = "Customer";
        public const string ControllerRoute = "v4/Customer";
        public const string GetActionText = "Get Customer";
        public const string GetAction = "Get";
        public const string PutAction = "Put";
        public const string PostAction = "Post";
        public const string DeleteAction = "Delete";

        /// <summary>
        /// Retrieves customer by Id
        /// </summary>
        /// <returns>Customer that matches the Id, or initialized CustomerModel for not found condition</returns>
        [HttpGet(ControllerRoute + "/{key}")]
        public IActionResult Get(string key)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetByIdOrKey(key);
            return Ok(customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <returns></returns>
        [HttpPut(ControllerRoute)]
        public async Task<IActionResult> Put([FromBody]CustomerModel model)
        {
            var customer = model.CastOrFill<CustomerInfo>();
            using (var writer = new StoredProcedureWriter<CustomerInfo>(customer, new CustomerSPConfig(customer)))
            {
                customer = await writer.SaveAsync();
            }
            return Ok(customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model">Full customer model worth of data with user changes</param>
        /// <returns>CustomerModel containing customer data</returns>
        [HttpPost(ControllerRoute)]
        public async Task<IActionResult> Post([FromBody]CustomerModel model)
        {
            var customer = model.CastOrFill<CustomerInfo>();
            using (var writer = new StoredProcedureWriter<CustomerInfo>(customer, new CustomerSPConfig(customer)))
            {
                customer = await writer.SaveAsync();
            }
            return Ok(customer.CastOrFill<CustomerModel>());
        }

        /// <summary>
        /// Saves changes to a Customer
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete(ControllerRoute + "/{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            var reader = new EntityReader<CustomerInfo>();
            var customer = reader.GetByIdOrKey(key);
            using (var writer = new StoredProcedureWriter<CustomerInfo>(customer, new CustomerSPConfig(customer)))
            {
                customer = await writer.DeleteAsync();
            }

            return Ok(customer.CastOrFill<CustomerModel>());
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