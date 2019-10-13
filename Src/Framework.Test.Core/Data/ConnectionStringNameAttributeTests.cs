using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreConnectionStringNameTests
    {
        private const string testValue = "DefaultConnection";
        private const string testValueNotFound = "NoConnection";
        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_ConnectionStringAttribute()
        {
            var testItem = new ClassWithConnectString();
            string result = testItem.GetAttributeValue<ConnectionStringName>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_ConnectionStringFromConfig()
        {
            var result = Defaults.String;
            var configManager =new ConfigurationManagerCore(ApplicationTypes.Native);
            var configConnectString = new ConnectionStringSafe();
            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(ClassWithConnectString));
            Assert.IsTrue(result != Defaults.String);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_ConnectionStringEntity()
        {
            var result = Defaults.String;
            var configManager =new ConfigurationManagerCore(ApplicationTypes.Native);
            var configConnectString = new ConnectionStringSafe();
            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(EntityWithConnectString));
            Assert.IsTrue(result != Defaults.String);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Core_Data_ConnectionStringDatabase()
        {
            var result = Defaults.String;
            var configManager =new ConfigurationManagerCore(ApplicationTypes.Native);
            var configConnectString = new ConnectionStringSafe();

            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(EntityWithConnectString));
            Assert.IsTrue(result != Defaults.String);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [ConnectionStringName(testValue)]
        internal class ClassWithConnectString
        {            
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [ConnectionStringName(testValue)]
        internal class EntityWithConnectString : EntityInfo<EntityWithConnectString>
        {
            /// <summary>
            /// ValidationRules and BusinessRules that ensure no dirty data is committed
            /// </summary>
            public override IList<IValidationRule<EntityWithConnectString>> Rules()
            { return new List<IValidationRule<EntityWithConnectString>>(); }
        }
    }
}
