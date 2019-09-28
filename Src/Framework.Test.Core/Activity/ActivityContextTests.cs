using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoodToCode.Framework.Activity;
using System.Linq;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests code first ActivityContext object saving activity to the database 
    /// </summary>
    [TestClass()]
    public partial class ActivityContextTests
    {
        /// <summary>
        /// Tests code first ActivityContext object saving activity to the database
        /// </summary>
        [TestMethod()]
        public void Core_Activity_ActivityContext()
        {
            var preSaveCount = Defaults.Integer;
            var postSaveCount = Defaults.Integer;
            var reader = new ActivityContextReader();
            var writer = new ActivityContextWriter();
            var activity = new ActivityContext() { IdentityUserName = "test" };
            var refreshed = new ActivityContext();

            preSaveCount = reader.GetAll().Count();
            activity = writer.Save(activity);
            postSaveCount = reader.GetAll().Count();
            Assert.IsTrue(activity.ActivityContextId != Defaults.Integer);
            Assert.IsTrue(postSaveCount == preSaveCount + 1);

            refreshed = reader.GetByKey(activity.ActivityContextKey);
            Assert.IsTrue(activity.ActivityContextId != Defaults.Integer);
            Assert.IsTrue(activity.ActivityContextKey == refreshed.ActivityContextKey);
        }
    }
}