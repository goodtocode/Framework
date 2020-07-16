
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Exersizes EntityReader functionality
    /// </summary>
    [TestClass()]
    public class CoreEntityReaderTests
    {
        /// <summary>
        /// Initializes class before tests are ran
        /// </summary>
        [ClassInitialize()]
        public static async Task ClassInit(TestContext context)
        {
            // Database is required for these tests
            var databaseAccess = false;
            var configuration = new ConfigurationManagerCore(ApplicationTypes.Native);
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
            // Need a few records to test with across all functions
            var customerTests = new CustomerInfoTests();
            await customerTests.Core_Entity_CustomerInfo_Insert();
            await customerTests.Core_Entity_CustomerInfo_Insert();
            await customerTests.Core_Entity_CustomerInfo_Insert();
            await customerTests.Core_Entity_CustomerInfo_Insert();
            await customerTests.Core_Entity_CustomerInfo_Insert();
        }

        /// <summary>
        /// Data_EntityReader_CountAny
        /// </summary>
        [TestMethod()]
        public void Core_Data_EntityReader_CountAny()
        {
            var db = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());

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
            Assert.IsTrue(key != Guid.Empty);

            // GetAll().Where count and any
            var resultsWhere = db.GetAll().Where(x => x.Key == key);
            Assert.IsTrue(resultsWhere.Count() > 0);
            Assert.IsTrue(resultsWhere.Any());
        }

        /// <summary>s
        /// Data_EntityReader_Select
        /// </summary>
        [TestMethod()]
        public void Core_Data_EntityReader_GetAll()
        {
            var typeDB = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var typeResults = typeDB.GetAll().Take(1);
            Assert.IsTrue(typeResults.Count() > 0);
        }

        /// <summary>
        /// Data_EntityReader_GetById
        /// </summary>
        [TestMethod()]
        public void Core_Data_EntityReader_GetById()
        {
            var custData = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
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
        public void Core_Data_EntityReader_GetByKey()
        {
            var custData = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());

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
        public void Core_Data_EntityReader_GetWhere()
        {
            // Plain EntityInfo object
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var testType = new CustomerInfo();            
            var testList = reader.GetAllExcludeDefault();
            var testItem = testList.FirstOrDefaultSafe();
            var testId = testItem.Id;
            testType = testList.Where(x => x.Id == testId).FirstOrDefaultSafe();
            Assert.IsTrue(testType.IsNew == false);
            Assert.IsTrue(testType.Id != -1);
            Assert.IsTrue(testType.Key != Guid.Empty);
        }

        /// <summary>
        /// Data_EntityReader_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Data_EntityReader_GetByPage()
        {
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var item = new CustomerInfo();
            var pageCount = 3;
            var pageCurrent = 1;
            var testKeys = reader.GetAll().Take(10).Select(x => x.Key);
            var firstSet = reader.GetByPage(x => testKeys.Contains(x.Key), y => y.Id, pageCount, pageCurrent).ToList();
            Assert.IsTrue(firstSet.Count() == pageCount);
            Assert.IsTrue(firstSet.First().Id <= firstSet.Last().Id);
            pageCurrent += 1;
            var secondSet = reader.GetByPage(x => testKeys.Contains(x.Key), y => y.Id, pageCount, pageCurrent).ToList();
            Assert.IsTrue(secondSet.Count() == pageCount);
            Assert.IsTrue(secondSet.First().Id <= secondSet.Last().Id);
            Assert.IsTrue(firstSet.Last().Id <= secondSet.First().Id);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Core_Data_EntityReader_Lists()
        {
            var emptyGuid = Guid.Empty;

            // List Type
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var typeResults = reader.GetAllExcludeDefault();
            Assert.IsTrue(typeResults.Count() > 0);
            Assert.IsTrue(typeResults.Any(x => x.Key == emptyGuid) == false);
            Assert.IsTrue(typeResults.Any(x => x.Id == -1) == false);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Core_Data_EntityReader_Singles()
        {
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());            
            var testItem = new CustomerInfo();
            var emptyGuid = Guid.Empty;

            // By Id
            var results = reader.GetAllExcludeDefault();
            var first = results.FirstOrDefaultSafe();
            testItem = reader.GetByKey(first.Key);
            Assert.IsTrue(testItem.IsNew == false);
            Assert.IsTrue(testItem.Id != -1);
            Assert.IsTrue(testItem.Key != Guid.Empty);

            // By Key
            testItem = reader.GetByKey(reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key);
            Assert.IsTrue(testItem.IsNew == false);
            Assert.IsTrue(testItem.Id != -1);
            Assert.IsTrue(testItem.Key != Guid.Empty);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static async Task Cleanup()
        {
            await CustomerInfoTests.Cleanup();
        }
    }
}
