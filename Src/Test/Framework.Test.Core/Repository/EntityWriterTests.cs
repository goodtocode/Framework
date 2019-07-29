//-----------------------------------------------------------------------
// <copyright file="EntityWriterTests.cs" company="GoodToCode">
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
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreEntityWriterTests
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
        List<CustomerEntity> testEntities = new List<CustomerEntity>()
        {
            new CustomerEntity() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerEntity() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerEntity() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerEntity() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerEntity() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
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
        /// Data_EntityWriter_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Data_EntityWriter_Insert()
        {
            EntityWriter<CustomerEntity> customerWriter;
            var testEntity = new CustomerEntity();
            var resultEntity = new CustomerEntity();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;
            var newId = Defaults.Integer;
            var newKey = Defaults.Guid;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Do Insert and check passed entity and returned entity
            customerWriter = new EntityWriter<CustomerEntity>();
            customerWriter.ConfigOptions.IgnoredProperties.Add(p => p.State );
            testEntity.ActivityContextKey = new ActivityContextReader().GetAll().Take(1).FirstOrDefaultSafe().ActivityContextKey;
            resultEntity = customerWriter.Save(testEntity);
            Assert.IsTrue(resultEntity.Id != Defaults.Integer);
            Assert.IsTrue(resultEntity.Key != Defaults.Guid);
        
            // Pull from DB and retest
            testEntity = new EntityReader<CustomerEntity>().GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Data_EntityWriter_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Data_EntityWriter_Update()
        {
            var testEntity = new CustomerEntity();
            var reader = new EntityReader<CustomerEntity>();
            var oldFirstName = Defaults.String;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            var entityId = Defaults.Integer;
            var entityKey = Defaults.Guid;

            // Create and capture original data
            Core_Data_EntityWriter_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do Update
            var writer = new EntityWriter<CustomerEntity>();
            writer.Save(testEntity);

            // Pull from DB and retest
            testEntity = reader.GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
        }

        /// <summary>
        /// Data_EntityWriter_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Data_EntityWriter_Delete()
        {
            var reader = new EntityReader<CustomerEntity>();
            var testEntity = new CustomerEntity();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;

            // Insert and baseline test
            Core_Data_EntityWriter_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do delete
            var writer = new EntityWriter<CustomerEntity>();
            writer.Delete(testEntity);

            // Pull from DB and retest
            testEntity = reader.GetAll().Where(x => x.Id == oldId).FirstOrDefaultSafe();
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Add to recycle bin for cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static void Cleanup()
        {
            var reader = new EntityReader<CustomerEntity>();
            var toDelete = new CustomerEntity();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefaultSafe();
                var db = new EntityWriter<CustomerEntity>();
                db.Delete(toDelete);
            }
        }
    }    
}
