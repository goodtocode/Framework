﻿
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

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
            
            using (var connection = new SqlConnection(new ConnectionStringFactory().GetDefaultConnection()))
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
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
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
        public async Task Core_Data_StoredProcedure_Update()
        {
            var testItem = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var customerTests = new CustomerInfoTests();

            await customerTests.Core_Entity_CustomerInfo_Insert();
            testItem = reader.GetByKey(CustomerTests.RecycleBin.LastOrDefault());

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
        public async Task Core_Data_StoredProcedure_Delete()
        {
            var testItem = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var customerTests = new CustomerInfoTests();

            await customerTests.Core_Entity_CustomerInfo_Insert();
            testItem = reader.GetByKey(CustomerTests.RecycleBin.LastOrDefault());
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
        public static async Task Cleanup()
        {            
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var toDelete = new CustomerInfo();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetByKey(item);
                using (var writer = new EntityWriter<CustomerInfo>(toDelete, new ConnectionStringFactory().GetDefaultConnection()))
                {
                    await writer.DeleteAsync();
                }
            }
        }
    }
}
