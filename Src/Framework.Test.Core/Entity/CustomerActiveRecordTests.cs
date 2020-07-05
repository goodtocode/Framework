
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CustomerActiveRecordTests
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
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
        }

        /// <summary>
        /// Core_Entity_CustomerActiveRecord_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerActiveRecord_Insert()
        {
            var testEntity = new CustomerActiveRecord();

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            var oldId = testEntity.Id;
            var oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Insert and check passed entity and returned entity
            var resultEntity = await testEntity.SaveAsync();
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);
            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            var refreshEntity = CustomerActiveRecord.GetById(resultEntity.Id);
            Assert.IsTrue(refreshEntity.IsNew == false);
            Assert.IsTrue(refreshEntity.Id != oldId);
            Assert.IsTrue(refreshEntity.Key != oldKey);
            Assert.IsTrue(refreshEntity.Id != -1);
            Assert.IsTrue(refreshEntity.Key != Guid.Empty);
            Assert.IsTrue(!refreshEntity.FailedRules.Any());

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerActiveRecord_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerActiveRecord_Update()
        {
            var testEntity = new CustomerActiveRecord();
            var oldFirstName = string.Empty;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            var entityId = -1;
            var entityKey = Guid.Empty;

            // Create and capture original data
            await Core_Entity_CustomerActiveRecord_Insert();
            testEntity.Fill(CustomerActiveRecord.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefault());
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Update
            var resultEntity = await testEntity.SaveAsync();
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            resultEntity = CustomerActiveRecord.GetById(entityId);
            Assert.IsTrue(resultEntity.IsNew == false);
            Assert.IsTrue(resultEntity.Id == entityId);
            Assert.IsTrue(resultEntity.Key == entityKey);
            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
            Assert.IsTrue(!resultEntity.FailedRules.Any());
        }

        /// <summary>
        /// Core_Entity_CustomerActiveRecord_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerActiveRecord_Delete()
        {            
            var oldId = -1;
            var oldKey = Guid.Empty;

            // Insert and baseline test
            await Core_Entity_CustomerActiveRecord_Insert();
            var testEntity = CustomerActiveRecord.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do delete
            var activeRecord = new CustomerActiveRecord();
            activeRecord.Fill(testEntity);
            var resultEntity = await activeRecord.DeleteAsync();
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = CustomerActiveRecord.GetAll().Where(x => x.Id == oldId).FirstOrDefault() ?? new CustomerInfo();
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Add to recycle bin for cleanup
            RecycleBin.Add(testEntity.Key);
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
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefault();
                using (var db = new EntityWriter<CustomerInfo>(toDelete, new CustomerSPConfig(toDelete)))
                {
                    await db.DeleteAsync();
                }
            }
        }
    }
}