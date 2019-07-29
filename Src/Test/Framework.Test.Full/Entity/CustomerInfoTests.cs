//-----------------------------------------------------------------------
// <copyright file="CustomerInfoTests.cs" company="GoodToCode">
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
using GoodToCode.Extras.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
using GoodToCode.Extras.Mathematics;
using GoodToCode.Framework.Repository;
using System.Data.SqlClient;
using GoodToCode.Extras.Configuration;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class FullCustomerInfoTests
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
            var configuration = new ConfigurationManagerFull();
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
        }

        /// <summary>
        /// Full_Entity_CustomerInfo_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_Insert()
        {
            var customerWriter = new StoredProcedureWriter<CustomerInfo>();
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
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Do Insert and check passed entity and returned entity
            resultEntity = customerWriter.Create(testEntity);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
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
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Insert_Id
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Entity_CustomerInfo_Insert_Id()
        {
            var customerWriter = new StoredProcedureWriter<CustomerInfo>();
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;
            var newId = Defaults.Integer;
            var newKey = Defaults.Guid;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, 5)]);
            testEntity.Id = Defaults.Integer;
            testEntity.Key = Defaults.Guid;
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key == Defaults.Guid);

            // Do Insert and check passed entity and returned entity
            resultEntity = customerWriter.Create(testEntity);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
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
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Insert_Id
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Entity_CustomerInfo_Insert_Key()
        {
            var customerWriter = new StoredProcedureWriter<CustomerInfo>();
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;
            var newId = Defaults.Integer;
            var newKey = Defaults.Guid;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, 5)]);
            testEntity.Id = Defaults.Integer;
            testEntity.Key = Guid.NewGuid();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.Id == Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do Insert and check passed entity and returned entity
            resultEntity = customerWriter.Create(testEntity);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
            Assert.IsTrue(resultEntity.Id != Defaults.Integer);
            Assert.IsTrue(resultEntity.Key != Defaults.Guid);

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key == oldKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Full_Entity_CustomerInfo_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_Update()
        {
            var testEntity = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>();
            var oldFirstName = Defaults.String;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            var entityId = Defaults.Integer;
            var entityKey = Defaults.Guid;

            // Create and capture original data
            Full_Entity_CustomerInfo_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do Update
            var writer = new StoredProcedureWriter<CustomerInfo>();
            writer.Update(testEntity);

            // Pull from DB and retest
            testEntity = reader.GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);
        }

        /// <summary>
        /// Full_Entity_CustomerInfo_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_Delete()
        {
            var reader = new EntityReader<CustomerInfo>();
            var testEntity = new CustomerInfo();
            var oldId = Defaults.Integer;
            var oldKey = Defaults.Guid;

            // Insert and baseline test
            Full_Entity_CustomerInfo_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != Defaults.Integer);
            Assert.IsTrue(testEntity.Key != Defaults.Guid);

            // Do delete
            var writer = new StoredProcedureWriter<CustomerInfo>();
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
        /// Basic class tests
        /// </summary>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_Class()
        {
            var searchChar = "i";
            var originalObject = new CustomerInfo() { FirstName = searchChar, LastName = searchChar };
            var resultObject = new CustomerInfo();
            var resultString = Defaults.String;
            var serializer = new JsonSerializer<CustomerInfo>();

            resultString = serializer.Serialize(originalObject);
            Assert.IsTrue(resultString != Defaults.String);
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar);
            Assert.IsTrue(resultObject.LastName == searchChar);
        }

        /// <summary>
        /// ISO8601 date tests
        /// </summary>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_ISO8601()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerModel>();
            var resultObject = new CustomerModel();
            var resultString = Defaults.String;
            var zeroTime = Defaults.Date;
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);
            var noMS = new DateTime(1983, 12, 9, 5, 10, 20, 000);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 2 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(30);
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 3 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(300);
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // Mixed
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0), ModifiedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);
        }

        /// <summary>
        /// ISO 8601F tests
        /// </summary>
        [TestMethod()]
        public void Full_Entity_CustomerInfo_ISO8601F()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerModel>();
            var resultObject = new CustomerModel();
            var resultString = Defaults.String;
            var zeroTime = Defaults.Date;
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601F) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);

            // 2 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30);
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 2);

            // 3 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300);
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 3);

            // Mixed
            resultObject = new CustomerModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS.AddMilliseconds(-testMS.Millisecond), CreatedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30), ModifiedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS.AddMilliseconds(-testMS.Millisecond) && resultObject.CreatedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30) && resultObject.ModifiedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300));
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static void Cleanup()
        {
            var reader = new EntityReader<CustomerInfo>();
            var toDelete = new CustomerInfo();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefaultSafe();
                var db = new StoredProcedureWriter<CustomerInfo>();
                db.Delete(toDelete);
            }
        }
    }
}