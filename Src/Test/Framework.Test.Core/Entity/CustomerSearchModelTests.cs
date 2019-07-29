//-----------------------------------------------------------------------
// <copyright file="CustomerSearchModelTests.cs" company="GoodToCode">
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
using GoodToCode.Extras.Mathematics;
using GoodToCode.Extras.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CustomerSearchModelTests
    {
        readonly List<CustomerSearchModel> testEntities = new List<CustomerSearchModel>()
        {
            new CustomerSearchModel() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerSearchModel() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerSearchModel() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerSearchModel() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerSearchModel() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
        };

        /// <summary>
        /// Basic class tests
        /// </summary>
        [TestMethod()]
        public void Core_Entity_CustomerSearchModel_Class()
        {
            var searchChar = "i";
            var originalObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar };
            var resultObject = new CustomerSearchModel();
            var resultString = Defaults.String;
            var serializer = new JsonSerializer<CustomerSearchModel>();

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
        public void Core_Entity_CustomerSearchModel_ISO8601()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerSearchModel>();
            var resultObject = new CustomerSearchModel();
            var resultString = Defaults.String;
            var zeroTime = Defaults.Date;
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);
            var noMS = new DateTime(1983, 12, 9, 5, 10, 20, 000);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 2 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(30);
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 3 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(300);
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // Mixed
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0), ModifiedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0) };
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
        public void Core_Entity_CustomerSearchModel_ISO8601F()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerSearchModel>();
            var resultObject = new CustomerSearchModel();
            var resultString = Defaults.String;
            var zeroTime = Defaults.Date;
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601F) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);

            // 2 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30);
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 2);

            // 3 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300);
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 3);

            // Mixed
            resultObject = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS.AddMilliseconds(-testMS.Millisecond), CreatedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30), ModifiedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != Defaults.String);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS.AddMilliseconds(-testMS.Millisecond) && resultObject.CreatedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30) && resultObject.ModifiedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300));
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);
        }
    }
}