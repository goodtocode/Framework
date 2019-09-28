using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Extensions.Text;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreJsonSerializerTests
    {
        [TestMethod()]
        public void Core_Serialization_Json_ValueTypes()
        {
            var data1 = Defaults.String;
            var testData1 = "TestDataHere";
            ISerializer<object> serialzer1 = new JsonSerializer<object>();
            data1 = serialzer1.Serialize(testData1);
            Assert.IsTrue(serialzer1.Deserialize(data1).ToString() == testData1);
            
            var data = Defaults.String;
            StringMutable testData = "TestDataHere";
            var serialzer = new JsonSerializer<StringMutable>();
            data = serialzer.Serialize(testData);
            Assert.IsTrue(serialzer.Deserialize(data).ToString() == testData.ToString());
        }

        [TestMethod()]
        public void Core_Serialization_Json_ReferenceTypes()
        {
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }
    }
}