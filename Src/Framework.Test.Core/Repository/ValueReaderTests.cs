
using GoodToCode.Extensions;
using GoodToCode.Framework.Value;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            using (var db = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection()))
            {

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
                Assert.IsTrue(key != Guid.Empty);

                // GetAll().Where count and any
                var resultsWhere = db.GetAll().Where(x => x.Key == key);
                Assert.IsTrue(resultsWhere.Count() > 0);
                Assert.IsTrue(resultsWhere.Any());
            }
        }

        /// <summary>
        /// Data_ValueReader_Select
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_GetAll()
        {
            using (var typeDB = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection()))
            {
                var typeResults = typeDB.GetAll().Take(1);
                Assert.IsTrue(typeResults.Count() > 0);
            }            
        }

        /// <summary>
        /// Data_ValueReader_GetByKey
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_GetByKey()
        {
            var custData = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection());

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
            var typeData = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection());
            var testType = new CustomerType();            
            var testList = typeData.GetAllExcludeDefault();
            var testItem = testList.FirstOrDefaultSafe();
            var testKey = testItem.Key;
            testType = testList.Where(x => x.Key == testKey).FirstOrDefaultSafe();
            Assert.IsTrue(testType.Key != Guid.Empty);
        }

        /// <summary>
        /// ValueReader context and connection
        /// </summary>
        [TestMethod()]
        public void Core_Data_ValueReader_Lists()
        {
            var emptyGuid = Guid.Empty;

            // List Type
            var typeDB = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection());
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
            var reader = new ValueReader<CustomerType>(new ConnectionStringFactory().GetDefaultConnection());            
            var testItem = new CustomerType();
            var emptyGuid = Guid.Empty;

            // By Key
            testItem = reader.GetByKey(reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key);
            Assert.IsTrue(testItem.Key != Guid.Empty);
        }
    }
}
