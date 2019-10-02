//-----------------------------------------------------------------------
// <copyright file="JsonSerializerTests.cs" company="GoodToCode">
//      Copyright (c) 2017-2018 GoodToCode. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Extensions.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Framework.Test
{
    [TestClass()]
    public class JsonSerializerTests
    {
        [TestMethod()]
        public void Serialization_Json_ValueTypes()
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
        public void Serialization_Json_ReferenceTypes()
        {
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }
    }
}