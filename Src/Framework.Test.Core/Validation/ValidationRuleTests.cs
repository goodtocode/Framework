using GoodToCode.Framework.Validation;
using GoodToCode.Framework.Worker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreValidationRuleTests
    {
        [TestMethod()]
        public void Core_Validation_ValidationRule()
        {
            WorkerResult result = new WorkerResult() { ReturnId = 123 };            
            ValidationRule<WorkerResult> rule = new ValidationRule<WorkerResult>(x => x.ReturnId > 0);
            Assert.IsTrue(rule.Validate(result) == true, "Did not work");
        }

        [TestMethod()]
        public void Core_Validation_Validate()
        {
            WorkerResult result = new WorkerResult() { ReturnId = 123 };
            ValidationRule<WorkerResult> rule = new ValidationRule<WorkerResult>(x => x.ReturnId > 0);
            Assert.IsTrue(rule.Validate(result) == true, "Did not work");
        }
    }
}