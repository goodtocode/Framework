
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Extensions.Text;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreStoredProcedureEntityTests
    {
        private static readonly object LockObject = new object();
        private static volatile List<Guid> _recycleBin = null;
        /// <summary>
        /// Singleton for recycle bin
        /// </summary>
        private static List<Guid> RecycleBin
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
            var configuration = new ConfigurationManagerCore(ApplicationTypes.Native);
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess, @"App_Data\\ConnectionStrings.config DefaultConnection is not able to connect to SQL Server. Please check your connection string and try again.");
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_Save()
        {
            var db = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var newCustomer = new CustomerInfo();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();

            // Create should update original object, and pass back a fresh-from-db object
            newCustomer.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);

            using (var writer = new EntityWriter<CustomerInfo>(newCustomer, new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(), newCustomer)))
            {
                resultCustomer = await writer.SaveAsync();
            }
            Assert.IsTrue(newCustomer.Key != Guid.Empty);
            Assert.IsTrue(resultCustomer.Id != -1);
            Assert.IsTrue(resultCustomer.Key != Guid.Empty);

            // Object in db should match in-memory objects
            dbCustomer = db.GetById(resultCustomer.Id);
            Assert.IsTrue(dbCustomer.Id != -1);
            Assert.IsTrue(dbCustomer.Key != Guid.Empty);
            Assert.IsTrue(dbCustomer.Id == resultCustomer.Id);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key);

            RecycleBin.Add(newCustomer.Key);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_Create()
        {
            var db = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var newCustomer = new CustomerInfo();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();

            // Create should update original object, and pass back a fresh-from-db object
            newCustomer.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            using (var writer = new EntityWriter<CustomerInfo>(newCustomer, new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(), newCustomer)))
            {
                resultCustomer = await writer.CreateAsync();
            }
            Assert.IsTrue(newCustomer.Key != Guid.Empty);
            Assert.IsTrue(resultCustomer.Id != -1);
            Assert.IsTrue(resultCustomer.Key != Guid.Empty);

            // Object in db should match in-memory objects
            dbCustomer = db.GetById(resultCustomer.Id);
            Assert.IsTrue(dbCustomer.Id != -1);
            Assert.IsTrue(dbCustomer.Key != Guid.Empty);
            Assert.IsTrue(dbCustomer.Id == resultCustomer.Id);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key);

            RecycleBin.Add(newCustomer.Key);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_Read()
        {
            var db = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var dbCustomer = new CustomerInfo();
            var lastKey = Guid.Empty;

            await Core_Entity_StoredProcedureEntity_Create();
            lastKey = RecycleBin.Last();

            dbCustomer = db.GetByKey(lastKey);
            Assert.IsTrue(dbCustomer.Id != -1);
            Assert.IsTrue(dbCustomer.Key != Guid.Empty);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_Update()
        {
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var item = new CustomerInfo();
            var resultCustomer = new CustomerInfo();
            var uniqueValue = RandomString.Next();
            var lastKey = Guid.Empty;
            var originalID = -1;
            var originalKey = Guid.Empty;

            await Core_Entity_StoredProcedureEntity_Create();
            lastKey = RecycleBin.Last();

            item = reader.GetByKey(lastKey);
            originalID = item.Id;
            originalKey = item.Key;
            Assert.IsTrue(item.Id != -1);
            Assert.IsTrue(item.Key != Guid.Empty);

            item.FirstName = uniqueValue;
            using (var writer = new EntityWriter<CustomerInfo>(item, new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(), item)))
            {
                resultCustomer = await writer.UpdateAsync();
            }
            Assert.IsTrue(resultCustomer.Id != -1);
            Assert.IsTrue(resultCustomer.Key != Guid.Empty);
            Assert.IsTrue(item.Id == resultCustomer.Id && resultCustomer.Id == originalID);
            Assert.IsTrue(item.Key == resultCustomer.Key && resultCustomer.Key == originalKey);

            item = reader.GetById(originalID);
            Assert.IsTrue(item.Id == resultCustomer.Id && resultCustomer.Id == originalID);
            Assert.IsTrue(item.Key == resultCustomer.Key && resultCustomer.Key == originalKey);

            Assert.IsTrue(item.Id != -1);
            Assert.IsTrue(item.Key != Guid.Empty);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_Delete()
        {
            var db = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var testItem = new CustomerInfo();
            var testResult = new CustomerInfo();
            var lastKey = Guid.Empty;
            var originalID = -1;
            var originalKey = Guid.Empty;

            await Core_Entity_StoredProcedureEntity_Create();
            lastKey = RecycleBin.Last();

            testItem = db.GetByKey(lastKey);
            originalID = testItem.Id;
            originalKey = testItem.Key;
            Assert.IsTrue(testItem.Id != -1);
            Assert.IsTrue(testItem.Key != Guid.Empty);
            Assert.IsTrue(testItem.CreatedDate.Date == DateTime.UtcNow.Date);

            using (var writer = new EntityWriter<CustomerInfo>(testItem, new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testItem)))
            {
                var deleteResult = await writer.DeleteAsync();
                Assert.IsTrue(deleteResult.IsNew);
            }

            testItem = db.GetById(originalID);
            Assert.IsTrue(testItem.Id != originalID);
            Assert.IsTrue(testItem.Key != originalKey);
            Assert.IsTrue(testItem.Id == -1);
            Assert.IsTrue(testItem.Key == Guid.Empty);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity_GetById
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_GetById()
        {
            var dbReader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var dbCustomer = new CustomerInfo();
            var lastKey = Guid.Empty;

            await Core_Entity_StoredProcedureEntity_Create();
            lastKey = RecycleBin.Last();

            dbCustomer = dbReader.GetByKey(lastKey);
            Assert.IsTrue(dbCustomer.Id != -1);
            Assert.IsTrue(dbCustomer.Key != Guid.Empty);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity_GetByKey
        /// </summary>
        [TestMethod()]
        public async Task Core_Entity_StoredProcedureEntity_GetByKey()
        {
            var dbCustomer = new CustomerInfo();
            var dbReader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var lastKey = Guid.Empty;

            await Core_Entity_StoredProcedureEntity_Create();
            lastKey = dbReader.GetByKey(RecycleBin.Last()).Key;

            dbCustomer = dbReader.GetByKey(lastKey);
            Assert.IsTrue(dbCustomer.Id != -1);
            Assert.IsTrue(dbCustomer.Key != Guid.Empty);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static async Task Cleanup()
        {            
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            foreach (Guid item in RecycleBin)
            {
                var toDelete = reader.GetByKey(item);
                using (var writer = new EntityWriter<CustomerInfo>(toDelete, new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  toDelete)))
                {
                    await writer.DeleteAsync();
                }
            }
        }
    }
}
