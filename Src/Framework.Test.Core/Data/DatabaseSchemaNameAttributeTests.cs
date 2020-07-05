
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreDatabaseSchemaNameTests
    {
        private const string testValue = "dbo";
        private const string testValueNotFound = "NoSchema";
        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_ConnectionStringAttribute()
        {
            var testItem = new ClassWithDatabaseSchema();
            string result = testItem.GetAttributeValue<DatabaseSchemaName>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_DatabaseSchemaAttribute()
        {
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [DatabaseSchemaName(testValue)]
        internal class ClassWithDatabaseSchema
        {
        }
    }
}
