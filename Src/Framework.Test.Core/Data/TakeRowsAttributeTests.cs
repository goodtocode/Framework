
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreTakeRowsTests
    {
        private const int testValue = 25;
        private const int testValueNotFound = 100;
        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_TakeRowsAttribute()
        {
            var testItem = new ClassWithTakeRows();
            int result = testItem.GetAttributeValue<TakeRows, int>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [TakeRows(testValue)]
        internal class ClassWithTakeRows
        {            
        }
    }
}
