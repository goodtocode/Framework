using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Extensions.Text;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreSerializerGenericTests
    {

        [TestMethod()]
        public void Core_Serialization_SerializerGeneric_ValueTypes()
        {
            // Immutable string class
            var data1 = Defaults.String;
            var Testdata1 = "TestDataHere";
            ISerializer<object> Serialzer1 = new JsonSerializer<object>();
            data1 = Serialzer1.Serialize(Testdata1);
            Assert.IsTrue(Serialzer1.Deserialize(data1).ToString() == Testdata1);

            
            var data = Defaults.String;
            StringMutable TestData = "TestDataHere";
            var Serialzer = new JsonSerializer<StringMutable>();
            data = Serialzer.Serialize(TestData);
            Assert.IsTrue(Serialzer.Deserialize(data).ToString() == TestData.ToString());
        }

        [TestMethod()]
        public void Core_Serialization_SerializerGeneric_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var serializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(serializedDataL).Count);
        }
    }
}