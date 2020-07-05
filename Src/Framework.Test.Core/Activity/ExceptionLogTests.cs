using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodToCode.Framework.Activity;
using System.Linq;


namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests code first ExceptionLog object saving activity to the database 
    /// </summary>
    [TestClass()]
    public class ExceptionLogTests
    {
        /// <summary>
        /// Tests code first ExceptionLog object saving activity to the database
        /// </summary>
        [TestMethod()]
        public void Core_Activity_ExceptionLog()
        {
            var preSaveCount = -1;
            var postSaveCount = -1;
            var reader = new ExceptionLogReader();
            var writer = new ExceptionLogWriter();

            preSaveCount = reader.GetAll().Count();
            var testItem = new ExceptionLog() { CustomMessage = "test" };
            testItem = writer.Save(testItem);
            postSaveCount = reader.GetAll().Count();
            Assert.IsTrue(testItem.ExceptionLogId != -1);
            Assert.IsTrue(postSaveCount == preSaveCount + 1);
        }
    }
}