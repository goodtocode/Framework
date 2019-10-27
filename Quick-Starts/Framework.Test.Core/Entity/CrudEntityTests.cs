//-----------------------------------------------------------------------
// <copyright file="StoredProcedureEntityTests.cs" company="GoodToCode">
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
    public class StoredProcedureEntityTests
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
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Entity_StoredProcedureEntity_Create()
        {
            var newCustomer = new CustomerInfo();
            var resultEntity = new CustomerInfo();

            // Create should update original object, and pass back a fresh-from-db object
            newCustomer.Fill(testEntities[Arithmetic.Random(1, 5)]);
            using (var writer = new StoredProcedureWriter<CustomerInfo>(newCustomer, new CustomerSPConfig(newCustomer)))
            {
                resultEntity = await writer.CreateAsync();
            }

            Assert.IsTrue(newCustomer.Key != Defaults.Guid);
            Assert.IsTrue(resultEntity.Id != Defaults.Integer);
            Assert.IsTrue(resultEntity.Key != Defaults.Guid);

            // Object in db should match in-memory objects
            var testEntity = new EntityReader<CustomerInfo>().GetById(resultEntity.Id);
            Assert.IsTrue(!testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
            Assert.IsTrue(testEntity.Id == resultEntity.Id);
            Assert.IsTrue(testEntity.Key == resultEntity.Key && resultEntity.Key == newCustomer.Key);

            StoredProcedureEntityTests.RecycleBin.Add(newCustomer.Key);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Entity_StoredProcedureEntity_Update()
        {
            var reader = new EntityReader<CustomerInfo>();
            var resultEntity = new CustomerInfo();
            var uniqueValue = Guid.NewGuid().ToString().Replace("-", "");

            await Entity_StoredProcedureEntity_Create();
            var lastKey = StoredProcedureEntityTests.RecycleBin.Last();

            var testEntity = reader.GetByKey(lastKey);
            var originalId = testEntity.Id;
            var originalKey = testEntity.Key;
            Assert.IsTrue(!testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            testEntity.FirstName = uniqueValue;
            using (var writer = new StoredProcedureWriter<CustomerInfo>(testEntity, new CustomerSPConfig(testEntity)))
            {
                resultEntity = await writer.UpdateAsync();
            }
            Assert.IsTrue(!resultEntity.IsNew);
            Assert.IsTrue(resultEntity.Id != Defaults.Integer);
            Assert.IsTrue(resultEntity.Key != Defaults.Guid);
            Assert.IsTrue(testEntity.Id == resultEntity.Id && resultEntity.Id == originalId);
            Assert.IsTrue(testEntity.Key == resultEntity.Key && resultEntity.Key == originalKey);

            testEntity = reader.GetById(originalId);
            Assert.IsTrue(!testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == resultEntity.Id && resultEntity.Id == originalId);
            Assert.IsTrue(testEntity.Key == resultEntity.Key && resultEntity.Key == originalKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Entity_StoredProcedureEntity_Delete()
        {
            var reader = new EntityReader<CustomerInfo>();

            await Entity_StoredProcedureEntity_Create();
            var lastKey = StoredProcedureEntityTests.RecycleBin.Last();

            var testEntity = reader.GetByKey(lastKey);
            var originalId = testEntity.Id;
            Assert.IsTrue(!testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
            Assert.IsTrue(testEntity.CreatedDate.Date == DateTime.UtcNow.Date);

            using (var writer = new StoredProcedureWriter<CustomerInfo>(testEntity, new CustomerSPConfig(testEntity)))
            {
                testEntity = await writer.DeleteAsync();
            }
            Assert.IsTrue(testEntity.IsNew);

            testEntity = reader.GetById(originalId);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);
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
