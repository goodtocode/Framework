using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreDataAccessBehaviorNameTests
    {
        private const DataAccessBehaviors testValue = DataAccessBehaviors.SelectOnly;
        private const DataAccessBehaviors testValueNotFound = DataAccessBehaviors.AllAccess;

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_DataAccessBehaviorAttribute()
        {
            var testItem = new ClassWithDataAccessBehavior();
            DataAccessBehaviors result = testItem.GetAttributeValue<DataAccessBehavior, DataAccessBehaviors>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [DataAccessBehavior(testValue)]
        internal class ClassWithDataAccessBehavior
        {            
        }
    }
}
