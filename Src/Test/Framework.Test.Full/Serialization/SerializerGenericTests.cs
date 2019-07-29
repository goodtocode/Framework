//-----------------------------------------------------------------------
// <copyright file="SerializerGenericTests.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using GoodToCode.Extensions;
using GoodToCode.Extras.Serialization;
using GoodToCode.Extras.Text;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class FullSerializerGenericTests
    {

        [TestMethod()]
        public void Full_Serialization_SerializerGeneric_ValueTypes()
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
        public void Full_Serialization_SerializerGeneric_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var serializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(serializedDataL).Count);
        }
    }
}