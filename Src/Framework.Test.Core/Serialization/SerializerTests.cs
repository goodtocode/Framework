
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

using GoodToCode.Extensions.Serialization;
using GoodToCode.Extensions.Text;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreSerializerTests
    {
        [TestMethod()]
        public void Core_Serialization_Serializer_ValueTypes()
        {
            // Immutable string class
            var data1 = string.Empty;
            var TestData1 = "TestDataHere";
            ISerializer<object> serialzer1 = new JsonSerializer<object>();
            data1 = serialzer1.Serialize(TestData1);
            Assert.IsTrue(serialzer1.Deserialize(data1).ToString() == TestData1);

            
            var data = string.Empty;
            StringMutable testData = "TestDataHere";
            var Serialzer = new JsonSerializer<StringMutable>();
            data = Serialzer.Serialize(testData);
            Assert.IsTrue(Serialzer.Deserialize(data).ToString() == testData.ToString());
        }

        [TestMethod()]
        public void Core_Serialization_Serializer_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }
    }
}