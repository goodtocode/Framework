using GoodToCode.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreValidatorTests
    {
        [TestMethod()]
        public void Core_Validator_Validate()
        {
            var validator = new EntityValidator<CustomerInfo>(new CustomerInfo());
            var failedRules = validator.Validate();
            Assert.IsTrue(failedRules.Any());
        }
    }
}