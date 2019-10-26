//-----------------------------------------------------------------------
// <copyright file="DatabaseWriterTests.cs" company="GoodToCode">
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
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Test
{
    [TestClass()]
    public class DatabaseWriterTests
    {
        private static readonly object LockObject = new object();
        private static volatile List<Guid> _recycleBin = null;
        /// <summary>
        /// Singleton for recycle bin
        /// </summary>
        private static List<Guid> RecycleBin
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

        readonly List<CustomerInfo> testEntities = new List<CustomerInfo>()
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
            Assert.IsTrue(databaseAccess);
        }

        /// <summary>
        /// Data_DatabaseWriter_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Data_DatabaseWriter_Insert()
        {
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;
            var newId = Defaults.Integer;
            var newKey = Defaults.Guid;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, 5)]);
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Do Insert and check passed entity and returned entity            
            using (var writer = new StoredProcedureWriter<CustomerInfo>(testEntity, new CustomerSPConfig(testEntity)))
            {
                resultEntity = await writer.CreateAsync();
            }
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
            Assert.IsTrue(resultEntity.Id != Defaults.Integer);
            Assert.IsTrue(resultEntity.Key != Defaults.Guid);

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Cleanup
            DatabaseWriterTests.RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Data_DatabaseWriter_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Data_DatabaseWriter_Update()
        {
            var testEntity = new CustomerInfo();
            var oldFirstName = Defaults.String;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            int entityId = Defaults.Integer;
            var entityKey = Defaults.Guid;

            // Create and capture original data
            await Data_DatabaseWriter_Insert();
            testEntity = new EntityReader<CustomerInfo>().GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do Update
            using (var writer = new StoredProcedureWriter<CustomerInfo>(testEntity, new CustomerSPConfig(testEntity)))
            {
                testEntity = await writer.SaveAsync();
            }

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
        }

        /// <summary>
        /// Data_DatabaseWriter_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Data_DatabaseWriter_Delete()
        {
            var testEntity = new CustomerInfo();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;

            // Insert and baseline test
            await Data_DatabaseWriter_Insert();
            testEntity = new EntityReader<CustomerInfo>().GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do delete
            using (var writer = new StoredProcedureWriter<CustomerInfo>(testEntity, new CustomerSPConfig(testEntity)))
            {
                testEntity = await writer.DeleteAsync();
            }

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(oldId);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Add to recycle bin for cleanup
            DatabaseWriterTests.RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static async Task Cleanup()
        {
            var reader = new EntityReader<CustomerInfo>();
            var toDelete = new CustomerInfo();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefaultSafe();
                using (var db = new EntityWriter<CustomerInfo>(toDelete))
                {
                    await db.DeleteAsync();
                }
            }
        }
    }    
}
