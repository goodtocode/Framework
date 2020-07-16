using GoodToCode.Extensions;
using GoodToCode.Extensions.Mathematics;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CustomerInfoTests
    {
        private string _connectionString = string.Empty;
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
        /// Constructor
        /// </summary>
        public CustomerInfoTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT ") ?? "Development")}.json")
                .AddJsonFile("appsettings.json")
                .Build();
            _connectionString = config["ConnectionStrings:DefaultConnection"];
        }

        /// <summary>
        /// Initializes class before tests are ran
        /// </summary>
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {

        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerInfo_Insert()
        {
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            var oldId = testEntity.Id;
            var oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Insert and check passed entity and returned entity
            var config = new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testEntity);
            using (var writer = new EntityWriter<CustomerInfo>(config))
            {
                resultEntity = await writer.CreateAsync();
            }
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection()).GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Insert_Id
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerInfo_Insert_Id()
        {
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            testEntity.Id = -1;
            testEntity.Key = Guid.Empty;
            var oldId = testEntity.Id;
            var oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key == Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Insert and check passed entity and returned entity
            using (var writer = new EntityWriter<CustomerInfo>(new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testEntity)))
            {
                resultEntity = await writer.CreateAsync();
            }
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection()).GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Insert_Id
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerInfo_Insert_Key()
        {
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, testEntities.Count)]);
            testEntity.Id = -1;
            testEntity.Key = Guid.NewGuid();
            var oldId = testEntity.Id;
            var oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.Id == -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Insert and check passed entity and returned entity
            using (var writer = new EntityWriter<CustomerInfo>(new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testEntity)))
            {
                resultEntity = await writer.CreateAsync();
            }
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(resultEntity.Id != -1);
            Assert.IsTrue(resultEntity.Key != Guid.Empty);
            Assert.IsTrue(!resultEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection()).GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key == oldKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Cleanup
            RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerInfo_Update()
        {
            var testEntity = new CustomerInfo();
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var oldFirstName = string.Empty;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            var entityId = -1;
            var entityKey = Guid.Empty;

            // Create and capture original data
            await Core_Entity_CustomerInfo_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do Update
            using (var writer = new EntityWriter<CustomerInfo>(new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testEntity)))
            {
                testEntity = await writer.UpdateAsync();
            }
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = reader.GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());
        }

        /// <summary>
        /// Core_Entity_CustomerInfo_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Core_Entity_CustomerInfo_Delete()
        {
            var reader = new EntityReader<CustomerInfo>(new ConnectionStringFactory().GetDefaultConnection());
            var testEntity = new CustomerInfo();
            var oldId = -1;
            var oldKey = Guid.Empty;

            // Insert and baseline test
            await Core_Entity_CustomerInfo_Insert();
            testEntity = reader.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != -1);
            Assert.IsTrue(testEntity.Key != Guid.Empty);
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Do delete
            using (var writer = new EntityWriter<CustomerInfo>(new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  testEntity)))
            {
                testEntity = await writer.DeleteAsync();
            }
            Assert.IsTrue(!testEntity.FailedRules.Any());

            // Pull from DB and retest
            testEntity = reader.GetAll().Where(x => x.Id == oldId).FirstOrDefaultSafe();
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
        /// Basic class tests
        /// </summary>
        [TestMethod()]
        public void Core_Entity_CustomerInfo_Class()
        {
            var searchChar = "i";
            var originalObject = new CustomerInfo() { FirstName = searchChar, LastName = searchChar };
            var serializer = new JsonSerializer<CustomerInfo>();

            var resultString = serializer.Serialize(originalObject);
            Assert.IsTrue(resultString != string.Empty);
            var resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar);
            Assert.IsTrue(resultObject.LastName == searchChar);
        }

        /// <summary>
        /// ISO8601 date tests
        /// </summary>
        [TestMethod()]
        public void Core_Entity_CustomerInfo_ISO8601()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);
            var noMS = new DateTime(1983, 12, 9, 5, 10, 20, 000);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            var resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            var resultString = serializer.Serialize(resultObject);
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

        /// <summary>
        /// ISO 8601F tests
        /// </summary>
        [TestMethod()]
        public void Core_Entity_CustomerInfo_ISO8601F()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601F) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            var resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            var resultString = serializer.Serialize(resultObject);
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
                toDelete = reader.GetAll().Where(x => x.Key == item).FirstOrDefaultSafe();
                using (var db = new EntityWriter<CustomerInfo>(new CustomerSPConfig(new ConnectionStringFactory().GetDefaultConnection(),  toDelete)))
                {
                    await db.DeleteAsync();
                }
            }
        }
    }
}