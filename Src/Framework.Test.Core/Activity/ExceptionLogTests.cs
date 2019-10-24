using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodToCode.Framework.Activity;
using System.Linq;
using GoodToCode.Extensions;

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
            var preSaveCount = Defaults.Integer;
            var postSaveCount = Defaults.Integer;
            var reader = new ExceptionLogReader();
            var writer = new ExceptionLogWriter();

            preSaveCount = reader.GetAll().Count();
            var testItem = new ExceptionLog() { CustomMessage = "test" };
            testItem = writer.Save(testItem);
            postSaveCount = reader.GetAll().Count();
            Assert.IsTrue(testItem.ExceptionLogId != Defaults.Integer);
            Assert.IsTrue(postSaveCount == preSaveCount + 1);
        }
    }
}