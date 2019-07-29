//-----------------------------------------------------------------------
// <copyright file="StoredProcedureTests.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
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
using GoodToCode.Extras.Configuration;
using GoodToCode.Extras.Mathematics;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class StoredProcedureTests
    {
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

        List<CustomerInfo> testEntities = new List<CustomerInfo>()
        {
            new CustomerInfo() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
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
        /// Core_Data_CrudStoredProcedures_Construct
        /// </summary>
        [TestMethod()]
        public void Core_Data_StoredProcedure_Construct()
        {
            var testItem = new CustomerInfo();
            var insertProc = new StoredProcedure<CustomerInfo>()
            {
                StoredProcedureName = "CustomerInfoInsert",
                Parameters = new List<SqlParameter>()
                    {
                    new SqlParameter("@Key", testItem.Key),
                    new SqlParameter("@FirstName", testItem.FirstName),
                    new SqlParameter("@MiddleName", testItem.MiddleName),
                    new SqlParameter("@LastName", testItem.LastName),
                    new SqlParameter("@BirthDate", testItem.BirthDate),
                    new SqlParameter("@GenderId", testItem.GenderId),
                    new SqlParameter("@CustomerTypeId", testItem.CustomerTypeId)
                    }
            };
            var updateProc = new StoredProcedure<CustomerInfo>()
            {
                StoredProcedureName = "CustomerInfoUpdate",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Key", testItem.Key),
                    new SqlParameter("@FirstName", testItem.FirstName),
                    new SqlParameter("@MiddleName", testItem.MiddleName),
                    new SqlParameter("@LastName", testItem.LastName),
                    new SqlParameter("@BirthDate", testItem.BirthDate),
                    new SqlParameter("@GenderId", testItem.GenderId),
                    new SqlParameter("@CustomerTypeId", testItem.CustomerTypeId)
                }
            };

            Assert.IsTrue(insertProc.Parameters.Count() > 0);
            Assert.IsTrue(updateProc.Parameters.Count() > 0);
        }

        /// <summary>
        /// Core_Data_CrudStoredProcedures_Create
        /// </summary>
        [TestMethod()]
        public void Core_Data_StoredProcedure_Create()
        {
            var reader = new EntityReader<CustomerInfo>();
            var testData = "Test";
            var testItem = new CustomerInfo()
            {
                FirstName = testData,
                MiddleName = "Middle",
                LastName = "Last"
            };
            var insertProc = new StoredProcedure<CustomerInfo>()
            {
                StoredProcedureName = "CustomerInfoInsert",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Key", testItem.Key),
                    new SqlParameter("@FirstName", testItem.FirstName),
                    new SqlParameter("@MiddleName", testItem.MiddleName),
                    new SqlParameter("@LastName", testItem.LastName),
                    new SqlParameter("@BirthDate", testItem.BirthDate),
                    new SqlParameter("@GenderId", testItem.GenderId),
                    new SqlParameter("@CustomerTypeId", testItem.CustomerTypeId)
                }
            };

            Assert.IsTrue(insertProc.StoredProcedureName.Length > 0);
            Assert.IsTrue(insertProc.Parameters.Count() > 0);
        }

        /// <summary>
        /// Core_Data_CrudStoredProcedures_Update
        /// </summary>
        [TestMethod()]
        public void Core_Data_StoredProcedure_Update()
        {
            var testItem = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>();
            var customerTests = new CustomerInfoTests();

            customerTests.Core_Entity_CustomerInfo_Insert();
            testItem = reader.GetByKey(CustomerTests.RecycleBin.LastOrDefaultSafe());

            var updateProc = new StoredProcedure<CustomerInfo>()
            {
                StoredProcedureName = "CustomerInfoUpdate",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Key", testItem.Key),
                    new SqlParameter("@FirstName", testItem.FirstName),
                    new SqlParameter("@MiddleName", testItem.MiddleName),
                    new SqlParameter("@LastName", testItem.LastName),
                    new SqlParameter("@BirthDate", testItem.BirthDate),
                    new SqlParameter("@GenderId", testItem.GenderId),
                    new SqlParameter("@CustomerTypeId", testItem.CustomerTypeId)
                }
            };

            Assert.IsTrue(updateProc.StoredProcedureName.Length > 0);
            Assert.IsTrue(updateProc.Parameters.Count() > 0);
        }

        /// <summary>
        /// Core_Data_CrudStoredProcedures_Delete
        /// </summary>
        [TestMethod()]
        public void Core_Data_StoredProcedure_Delete()
        {
            var testItem = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>();
            var customerTests = new CustomerInfoTests();

            customerTests.Core_Entity_CustomerInfo_Insert();
            testItem = reader.GetByKey(CustomerTests.RecycleBin.LastOrDefaultSafe());
            var deleteProc = new StoredProcedure<CustomerInfo>()
            {
                StoredProcedureName = "CustomerInfoDelete",
                Parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Key", testItem.Key)
                }
            };

            Assert.IsTrue(deleteProc.StoredProcedureName.Length > 0);
            Assert.IsTrue(deleteProc.Parameters.Count() > 0);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static void Cleanup()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var reader = new EntityReader<CustomerInfo>();
            var toDelete = new CustomerInfo();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetByKey(item);
                writer.Delete(toDelete);
            }
        }
    }
}
