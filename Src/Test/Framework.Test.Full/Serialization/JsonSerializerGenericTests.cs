//-----------------------------------------------------------------------
// <copyright file="JsonSerializerGenericTests.cs" company="GoodToCode">
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
using System.Runtime.Serialization.Json;

namespace GoodToCode.Framework.Test
{
    public class PersonInfo
    {
        public string FirstName { get; set; } = Defaults.String;
        public string LastName { get; set; } = Defaults.String;
        public DateTime BirthDate { get; set; } = Defaults.Date;
    }

    [TestClass()]
    public class FullJsonSerializerGenericTests
    {
        private const string testPhrase = "Services up and running...";
        private const string testPhraseSerialized = "\"Services up and running...\"";
        private const string testPhraseMutableSerialized = "{\"Value\":\"Services up and running...\"}";

        [TestMethod()]
        public void Full_Serialization_Json_ValueTypes()
        {
            // Immutable string class
            var data1= Defaults.String;
            var Testdata1= "TestDataHere";
            ISerializer<object> Serialzer1 = new JsonSerializer<object>();
            data1= Serialzer1.Serialize(Testdata1);
            Assert.IsTrue(Serialzer1.Deserialize(data1).ToString() == Testdata1);
            
            var data = Defaults.String;
            StringMutable TestData = "TestDataHere";
            var Serialzer = new JsonSerializer<StringMutable>();
            data = Serialzer.Serialize(TestData);
            Assert.IsTrue(Serialzer.Deserialize(data).ToString() == TestData.ToString());
        }

        [TestMethod()]
        public void Full_Serialization_Json_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }

        [TestMethod()]
        public void Full_Serialization_Json_PersonInfo()
        {
            // Collections, etc
            var personObject = new PersonInfo() { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1977, 11, 20) };
            var personObjectSerialized = Defaults.String;
            var personDefaultWebAPI = "{\"BirthDate\":\"\\/Date(248860800000-0800)\\/\",\"FirstName\":\"John\",\"LastName\":\"Doe\"}";
            var personISO8601 = "{\"FirstName\":\"John\",\"MiddleName\":\"Michelle\",\"LastName\":\"Doe\",\"BirthDate\":\"1977-11-20T00:00:00\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\"}";
            var personISO8601F = "{\"FirstName\":\"John\",\"MiddleName\":\"Michelle\",\"LastName\":\"Doe\",\"BirthDate\":\"1977-11-20T00:00:00.000\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\"}";
            var personJsonReSerialized = Defaults.String;
            var personJsonDeserialized = new PersonInfo();
            var serializer = new JsonSerializer<PersonInfo>();

            // stringISODate -> object -> string
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601F);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);

            // ISO8601 (no milliseconds)
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));

            // Default: ISO8601F (with milliseconds)
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601F);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);

            // object -> string -> object
            personObjectSerialized = serializer.Serialize(personObject);
            Assert.IsTrue(personObjectSerialized.Length > 0);
            personObject = serializer.Deserialize(personObjectSerialized);
            Assert.IsTrue(personObject.FirstName == "John");
            Assert.IsTrue(personObject.LastName == "Doe");
            Assert.IsTrue(personObject.BirthDate == new DateTime(1977, 11, 20));

            // NOT SUPPORTED: stringNONISODate (default date) -> object -> string
            DataContractJsonSerializer defaultSerializer = new DataContractJsonSerializer(typeof(PersonInfo));
            serializer = new JsonSerializer<PersonInfo>();
            serializer.DateTimeFormatString = defaultSerializer.DateTimeFormat;
            personJsonDeserialized = serializer.Deserialize(personDefaultWebAPI);
            Assert.IsFalse(personJsonDeserialized.FirstName == "John");
            Assert.IsFalse(personJsonDeserialized.LastName == "Doe");
            Assert.IsFalse(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);
        }

        [TestMethod()]
        public void Full_Serialization_Json_String()
        {
            var serializer = new JsonSerializer<string>();

            Assert.IsTrue(testPhraseSerialized == serializer.Serialize(testPhrase));
            Assert.IsTrue(testPhrase == serializer.Deserialize(testPhraseSerialized));
        }

        [TestMethod()]
        public void Full_Serialization_Json_StringMutable()
        {
            StringMutable testPhraseMutable = testPhrase;
            var result = Defaults.String;
            StringMutable resultMutable = Defaults.String;
            var serializerMutable = new JsonSerializer<StringMutable>();
           
            // Serialization            
            testPhraseMutable = testPhrase;
            result = serializerMutable.Serialize(testPhraseMutable);
            Assert.IsTrue(result == testPhraseMutableSerialized);

            // Deserialization
            resultMutable = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(resultMutable == testPhrase);
        }

        [TestMethod()]
        public void Full_Serialization_Json_StringToStringMutable()
        {
            StringMutable testPhraseMutable = testPhrase;
            var result = Defaults.String;
            StringMutable resultMutable = Defaults.String;
            var serializerMutable = new JsonSerializer<StringMutable>();
            var serializer = new JsonSerializer<string>();

            // string Mutable can be serialized as string, then deserialized as string after transport 
            //  So that consumers don't need to know original was StringMutable
            result = serializer.Serialize(testPhraseMutable);
            Assert.IsTrue(testPhraseSerialized == result);

            // StringMutable serialize -> string deserialize
            result = serializerMutable.Deserialize(testPhraseSerialized); // Not supported scenario, should default ot empty string
            Assert.IsTrue(result == Defaults.String); 

            result = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(result == testPhrase);
            resultMutable = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(resultMutable == testPhrase);
        }
    }
}
