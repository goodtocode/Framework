
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Framework.Activity;
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

        readonly List<CustomerEntity> testEntities = new List<CustomerEntity>()
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
        public async Task Core_Data_EntityWriter_Insert()
        {
            var testEntity = new CustomerEntity();
            var resultEntity = new CustomerEntity();
            var oldId = -1;
            var oldKey = Guid.Empty;
            var newKey = Guid.Empty;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);

            // Do Insert and check passed entity and returned entity                        
            using (var writer = new EntityWriter<CustomerEntity>(testEntity, new ConnectionStringFactory().GetDefaultConnection()))
            {
                resultEntity = await writer.SaveAsync();
            }

            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
        
            // Pull from DB and retest
            testEntity = new EntityReader<CustomerEntity>(new ConnectionStringFactory().GetDefaultConnection()).GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Data_EntityWriter_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Data_EntityWriter_Update()
        {
            var testEntity = new CustomerEntity();
            var reader = new EntityReader<CustomerEntity>(new ConnectionStringFactory().GetDefaultConnection());
            var oldFirstName = string.Empty;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            var entityId = -1;
            var entityKey = Guid.Empty;

            // Create and capture original data
            await Core_Data_EntityWriter_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);

            // Do Update
            using (var writer = new EntityWriter<CustomerEntity>(testEntity, new ConnectionStringFactory().GetDefaultConnection()))
            {
                testEntity = await writer.SaveAsync();
            }

            // Pull from DB and retest
            testEntity = reader.GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
        }

        /// <summary>
        /// Data_EntityWriter_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Data_EntityWriter_Delete()
        {
            var reader = new EntityReader<CustomerEntity>(new ConnectionStringFactory().GetDefaultConnection());
            var testEntity = new CustomerEntity();
            var oldId = -1;
            var oldKey = Guid.Empty;

            // Insert and baseline test
            await Core_Data_EntityWriter_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);

            // Do delete
            using (var writer = new EntityWriter<CustomerEntity>(testEntity, new ConnectionStringFactory().GetDefaultConnection()))
            {
                testEntity = await writer.DeleteAsync();
            }

            // Pull from DB and retest
            testEntity = reader.GetAll().Where(x => x.Id == oldId).FirstOrDefaultSafe();
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);

            // Add to recycle bin for cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static async Task Cleanup()
        {
            var reader = new EntityReader<CustomerEntity>(new ConnectionStringFactory().GetDefaultConnection());
            var toDelete = new CustomerEntity();

            foreach (Guid item in RecycleBin)
            {
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefaultSafe();
                using (var db = new EntityWriter<CustomerEntity>(toDelete, new ConnectionStringFactory().GetDefaultConnection()))
                {
                    await db.DeleteAsync();
                }
            }
        }
    }    
}
