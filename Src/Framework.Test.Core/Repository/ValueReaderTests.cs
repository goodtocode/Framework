//-----------------------------------------------------------------------
// <copyright file="ValueReaderTests.cs" company="GoodToCode">
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
    public class CoreValueReaderTests
    {
        /// <summary>
        /// Data_ValueReader_CountAny
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_CountAny()
        {
            var db = new ValueReader<CustomerType>();

            // GetAll() count and any
            var resultsAll = db.GetAll();
            Assert.IsTrue(resultsAll.Count() > 0);
            Assert.IsTrue(resultsAll.Any());

            // GetAll().Take(1) count and any
            var resultsTake = db.GetAll().Take(1);
            Assert.IsTrue(resultsTake.Count() == 1);
            Assert.IsTrue(resultsTake.Any());

            // Get an Id to test
            var key = db.GetAllExcludeDefault().FirstOrDefaultSafe().Key;
            Assert.IsTrue(key != Defaults.Guid);

            // GetAll().Where count and any
            var resultsWhere = db.GetAll().Where(x => x.Key == key);
            Assert.IsTrue(resultsWhere.Count() > 0);
            Assert.IsTrue(resultsWhere.Any());
        }

        /// <summary>s
        /// Data_ValueReader_Select
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_GetAll()
        {
            var typeDB = new ValueReader<CustomerType>();
            var typeResults = typeDB.GetAll().Take(1);
            Assert.IsTrue(typeResults.Count() > 0);
        }

        /// <summary>
        /// Data_ValueReader_GetByKey
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_GetByKey()
        {
            var custData = new ValueReader<CustomerType>();

            // ByKey Should return 1 record
            var existingKey = custData.GetAll().FirstOrDefaultSafe().Key;
            var custWhereKey = custData.GetAll().Where(x => x.Key == existingKey);
            Assert.IsTrue(custWhereKey.Count() > 0);
        }

        /// <summary>
        /// Data_ValueReader_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Data_ValueReader_GetWhere()
        {
            // Plain EntityInfo object
            var typeData = new ValueReader<CustomerType>();
            var testType = new CustomerType();            
            var testList = typeData.GetAllExcludeDefault();
            var testItem = testList.FirstOrDefaultSafe();
            var testKey = testItem.Key;
            testType = testList.Where(x => x.Key == testKey).FirstOrDefaultSafe();
            Assert.IsTrue(testType.Key != Defaults.Guid);
        }

        /// <summary>
        /// ValueReader context and connection
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_Lists()
        {
            var emptyGuid = Defaults.Guid;

            // List Type
            var typeDB = new ValueReader<CustomerType>();
            var typeResults = typeDB.GetAllExcludeDefault();
            Assert.IsTrue(typeResults.Count() > 0);
            Assert.IsTrue(typeResults.Any(x => x.Key == emptyGuid) == false);
        }

        /// <summary>
        /// ValueReader context and connection
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_Singles()
        {
            var reader = new ValueReader<CustomerType>();            
            var testItem = new CustomerType();
            var emptyGuid = Defaults.Guid;

            // By Key
            testItem = reader.GetByKey(reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key);
            Assert.IsTrue(testItem.Key != Defaults.Guid);
        }
    }
}
