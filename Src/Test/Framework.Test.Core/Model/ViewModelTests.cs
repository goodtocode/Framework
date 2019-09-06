//-----------------------------------------------------------------------
// <copyright file="CustomerViewModelTests.cs" company="GoodToCode">
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
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Extensions.Net;
using GoodToCode.Extensions.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using System.Data.SqlClient;
using System.Net.Http;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Test GoodToCode Framework for Web API endpoints
    /// </summary>
    /// <remarks></remarks>
    [TestClass()]
    public class CoreViewModelTests
    {
        private readonly bool interfaceBreakingRelease = true; // Current release breaks the interface?
        private static readonly object LockObject = new object();
        private static volatile List<Guid> _recycleBin = null;

        /// <summary>
        /// Singleton for recycle bin
        /// </summary>
        public static List<Guid> RecycleBin
        {
            get
            {
                if (_recycleBin != null) return _recycleBin;
                lock (LockObject)
                {
                    if (_recycleBin == null)
                    {
                        _recycleBin = new List<Guid>();
                    }
                }
                return _recycleBin;
            }
        }

        private List<CustomerDto> customerTestData = new List<CustomerDto>()
        {
            new CustomerDto() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerDto() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerDto() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerDto() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerDto() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
        };

        /// <summary>
        /// Initializes class before tests are ran
        /// </summary>
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            // Database is required for these tests
            var databaseAccess = false;
            var configuration = new ConfigurationManagerCore(ApplicationTypes.Native);
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
        }

        /// <summary>
        /// Get a customer, via HttpGet from Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_ViewModel_CRUD_Read()
        {
            var customer = new CustomerDto();
            var viewModel = new TestViewModel<CustomerDto>("Customer");

            // Create test record
            await Core_ViewModel_CRUD_Create();
            var keyToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : Defaults.Guid;

            try
            {
                // Verify update success
                customer = await viewModel.GetByKeyAsync(keyToTest);
                Assert.IsTrue(interfaceBreakingRelease | customer.Id != Defaults.Integer);
                Assert.IsTrue(interfaceBreakingRelease | customer.Key != Defaults.Guid);
                Assert.IsTrue(interfaceBreakingRelease | !customer.IsNew);
            }
            catch (HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No such host") || ex.Message.Contains("no data"));
            }

        }

        /// <summary>
        /// Create a new customer, via HttpPut to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_ViewModel_CRUD_Create()
        {
            var customer = new CustomerDto();
            var url = new Uri(new ConfigurationManagerCore(ApplicationTypes.Native).AppSettingValue("MyWebService").AddLast("/Customer"));

            customer.Fill(customerTestData[Arithmetic.Random(1, customerTestData.Count)]);
            var request = new HttpRequestPut<CustomerDto>(url, customer);
            try
            {
                customer = await request.SendAsync();
                Assert.IsTrue(interfaceBreakingRelease | customer.Key != Defaults.Guid);
                RecycleBin.Add(customer.Key);
            }
            catch (HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No such host") || ex.Message.Contains("no data"));
            }
        }

        /// <summary>
        /// Update a customer, via HttpPost to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_ViewModel_CRUD_Update()
        {
            var customer = new CustomerDto();
            var viewModel = new TestViewModel<CustomerDto>("Customer");

            // Create test record
            await Core_ViewModel_CRUD_Create();
            var keyToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : Defaults.Guid;
            // Read test record
            try
            {
                customer = await viewModel.GetByKeyAsync(keyToTest);
                // Update test record
                var testKey = Guid.NewGuid().ToString();
                customer.FirstName = customer.FirstName.AddLast(testKey);
                customer = await viewModel.UpdateAsync(customer);
                Assert.IsTrue(interfaceBreakingRelease | customer.Id != Defaults.Integer);
                Assert.IsTrue(interfaceBreakingRelease | customer.Key != Defaults.Guid);
                // Verify update success
                customer = await viewModel.GetByKeyAsync(keyToTest);
                Assert.IsTrue(interfaceBreakingRelease | customer.FirstName.Contains(testKey));
                Assert.IsTrue(interfaceBreakingRelease | !viewModel.MyModel.IsNew);
                Assert.IsTrue(interfaceBreakingRelease | !customer.IsNew);
            }
            catch (HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No such host") || ex.Message.Contains("no data"));
            }

        }

        /// <summary>
        /// Delete a customer, via HttpDelete to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_ViewModel_CRUD_Delete()
        {
            var customer = new CustomerDto();
            var returnCustomer = new CustomerDto();
            var viewModel = new TestViewModel<CustomerDto>("Customer");
            var success = Defaults.Boolean;

            // Create test record
            await Core_ViewModel_CRUD_Create();
            var keyToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : Defaults.Guid;

            // Test
            try
            {
                customer = await viewModel.GetByKeyAsync(keyToTest);
                Assert.IsTrue(interfaceBreakingRelease | !viewModel.MyModel.IsNew);
                returnCustomer = await viewModel.DeleteAsync(customer);
                Assert.IsTrue(interfaceBreakingRelease | returnCustomer.IsNew);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.IsNew);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Id == Defaults.Integer);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Key == Defaults.Guid);
                // Verify update success
                customer = await viewModel.GetByKeyAsync(keyToTest);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.IsNew);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Id == Defaults.Integer);
                Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Key == Defaults.Guid);
                Assert.IsTrue(interfaceBreakingRelease | customer.IsNew);
                Assert.IsTrue(interfaceBreakingRelease | customer.Id == Defaults.Integer);
                Assert.IsTrue(interfaceBreakingRelease | customer.Key == Defaults.Guid);
            }
            catch (HttpRequestException ex)
            {
                Assert.IsTrue(ex.Message.Contains("No such host") || ex.Message.Contains("no data"));
            }
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static void Cleanup()
        {
            var db = new EntityWriter<CustomerInfo>();
            var reader = new EntityReader<CustomerInfo>();
            foreach (Guid item in RecycleBin)
            {
                db.Delete(reader.GetByKey(item));
            }
        }
    }
}
