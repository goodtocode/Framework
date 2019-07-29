//-----------------------------------------------------------------------
// <copyright file="EntityReaderTests.cs" company="GoodToCode">
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
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class FullEntityReaderTests
    {
        /// <summary>
        /// Data_EntityReader_CountAny
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_CountAny()
        {
            var db = new EntityReader<CustomerInfo>();

            // GetAll() count and any
            var resultsAll = db.GetAll();
            Assert.IsTrue(resultsAll.Count() > 0);
            Assert.IsTrue(resultsAll.Any());

            // GetAll().Take(1) count and any
            var resultsTake = db.GetAll().Take(1);
            Assert.IsTrue(resultsTake.Count() == 1);
            Assert.IsTrue(resultsTake.Any());

            // Get an Key to test
            var key = db.GetAllExcludeDefault().FirstOrDefaultSafe().Key;
            Assert.IsTrue(key != Defaults.Guid);

            // GetAll().Where count and any
            var resultsWhere = db.GetAll().Where(x => x.Key == key);
            Assert.IsTrue(resultsWhere.Count() > 0);
            Assert.IsTrue(resultsWhere.Any());
        }

        /// <summary>s
        /// Data_EntityReader_Select
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_GetAll()
        {
            var typeDB = new EntityReader<CustomerInfo>();
            var typeResults = typeDB.GetAll().Take(1);
            Assert.IsTrue(typeResults.Count() > 0);
        }

        /// <summary>
        /// Data_EntityReader_GetById
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_GetById()
        {
            var custData = new EntityReader<CustomerInfo>();
            var custEntity = new CustomerInfo();

            var existingId = custData.GetAllExcludeDefault().FirstOrDefaultSafe().Id;
            var custWhereId = custData.GetAll().Where(x => x.Id == existingId);
            Assert.IsTrue(custWhereId.Count() > 0);
            Assert.IsTrue(custWhereId.Any());

            custEntity = custWhereId.FirstOrDefaultSafe();
            Assert.IsTrue(custEntity.Id == existingId);
            Assert.IsTrue(custEntity.IsNew == false);
        }

        /// <summary>
        /// Data_EntityReader_GetByKey
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_GetByKey()
        {
            var custData = new EntityReader<CustomerInfo>();

            // ByKey Should return 1 record
            var existingKey = custData.GetAll().FirstOrDefaultSafe().Key;
            var custWhereKey = custData.GetAll().Where(x => x.Key == existingKey);
            Assert.IsTrue(custWhereKey.Count() > 0);
        }

        /// <summary>
        /// Data_EntityReader_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Full_Data_EntityReader_GetWhere()
        {
            // Plain EntityInfo object
            var typeData = new EntityReader<CustomerInfo>();
            var testType = new CustomerInfo();            
            var testList = typeData.GetAllExcludeDefault();
            var testItem = testList.FirstOrDefaultSafe();
            var testId = testItem.Id;
            testType = testList.Where(x => x.Id == testId).FirstOrDefaultSafe();
            Assert.IsTrue(testType.IsNew == false);
            Assert.IsTrue(testType.Id != Defaults.Integer);
            Assert.IsTrue(testType.Key != Defaults.Guid);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_Lists()
        {
            var emptyGuid = Defaults.Guid;

            // List Type
            var typeDB = new EntityReader<CustomerInfo>();
            var typeResults = typeDB.GetAllExcludeDefault();
            Assert.IsTrue(typeResults.Count() > 0);
            Assert.IsTrue(typeResults.Any(x => x.Key == emptyGuid) == false);
            Assert.IsTrue(typeResults.Any(x => x.Id == -1) == false);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Full_Data_EntityReader_Singles()
        {
            var reader = new EntityReader<CustomerInfo>();            
            var testItem = new CustomerInfo();
            var emptyGuid = Defaults.Guid;

            // Ignore properties that over complicate this test
            reader.ConfigOptions.IgnoredProperties.Add(p => p.ActivityContextKey);

            // By Id
            var results = reader.GetAllExcludeDefault();
            var first = results.FirstOrDefaultSafe();
            testItem = reader.GetByKey(first.Key);
            Assert.IsTrue(testItem.IsNew == false);
            Assert.IsTrue(testItem.Id != Defaults.Integer);
            Assert.IsTrue(testItem.Key != Defaults.Guid);

            // By Key
            testItem = reader.GetByKey(reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key);
            Assert.IsTrue(testItem.IsNew == false);
            Assert.IsTrue(testItem.Id != Defaults.Integer);
            Assert.IsTrue(testItem.Key != Defaults.Guid);
        }
    }
}
