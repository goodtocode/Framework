﻿
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CustomerTests
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

        readonly List<Customer> testEntities = new List<Customer>()
        {
            new Customer() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new Customer() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new Customer() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new Customer() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new Customer() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
        };

        /// <summary>
        /// Initializes class before tests are ran
        /// </summary>
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            // Database is required for these tests
            var databaseAccess = false;
            
            using (var connection = new SqlConnection(new ConnectionStringFactory().GetDefaultConnection()))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
        }

        [TestMethod()]
        public void Core_Entity_Customer_Class()
        {
            var searchChar = "i";
            var originalObject = new Customer() { FirstName = searchChar, LastName = searchChar };
            var resultObject = new Customer();
            var resultString = string.Empty;
            var serializer = new JsonSerializer<Customer>();

            resultString = serializer.Serialize(originalObject);
            Assert.IsTrue(resultString != string.Empty);
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar);
            Assert.IsTrue(resultObject.LastName == searchChar);
        }

        [TestMethod()]
        public void Core_Entity_Customer_ISO8601()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var resultObject = new CustomerDto();
            var resultString = string.Empty;
            var zeroTime = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);
            var noMS = new DateTime(1983, 12, 9, 5, 10, 20, 000);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 2 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(30);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 3 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(300);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // Mixed
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0), ModifiedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);
        }

        [TestMethod()]
        public void Core_Entity_Customer_ISO8601F()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var resultObject = new CustomerDto();
            var resultString = string.Empty;
            var zeroTime = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601F) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);

            // 2 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 2);

            // 3 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 3);

            // Mixed
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS.AddMilliseconds(-testMS.Millisecond), CreatedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30), ModifiedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS.AddMilliseconds(-testMS.Millisecond) && resultObject.CreatedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30) && resultObject.ModifiedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300));
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);
        }
    }
}