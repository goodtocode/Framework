using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoodToCode.Extensions.Serialization;
using System;
using System.Runtime.Serialization;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Test
{
    [TestClass()]
    public class CoreCustomerDtoTests
    {
        [TestMethod()]
        public void Core_Model_Customer_ISO8601()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);
            var noMS = new DateTime(1983, 12, 9, 5, 10, 20, 000);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            var resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            var resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 2 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(30);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // 3 digit millisecond
            testMS.AddMilliseconds(-testMS.Millisecond);
            testMS.AddMilliseconds(300);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);

            // Mixed
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0), ModifiedDate = new DateTime(1983, 12, 9, 5, 10, 20, 0) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == noMS && resultObject.CreatedDate == noMS && resultObject.ModifiedDate == noMS);
        }

        [TestMethod()]
        public void Core_Model_Customer_ISO8601F()
        {
            var searchChar = "i";
            var serializer = new JsonSerializer<CustomerDto>();
            var testMS = new DateTime(1983, 12, 9, 5, 10, 20, 3);

            //Explicitly set
            serializer.DateTimeFormatString = new DateTimeFormat(DateTimeExtension.Formats.ISO8601F) { DateTimeStyles = System.Globalization.DateTimeStyles.RoundtripKind };

            // 1 digit millisecond
            var resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            var resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);

            // 2 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 2);

            // 3 digit millisecond
            testMS = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300);
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS, CreatedDate = testMS, ModifiedDate = testMS };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS && resultObject.CreatedDate == testMS && resultObject.ModifiedDate == testMS);
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 3);

            // Mixed
            resultObject = new CustomerDto() { FirstName = searchChar, LastName = searchChar, BirthDate = testMS.AddMilliseconds(-testMS.Millisecond), CreatedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30), ModifiedDate = testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300) };
            resultString = serializer.Serialize(resultObject);
            Assert.IsTrue(resultString != string.Empty);
            Assert.IsTrue(resultString.Contains(testMS.ToString(DateTimeExtension.Formats.ISO8601F)));
            resultObject = serializer.Deserialize(resultString);
            Assert.IsTrue(resultObject.FirstName == searchChar && resultObject.LastName == searchChar);
            Assert.IsTrue(resultObject.BirthDate == testMS.AddMilliseconds(-testMS.Millisecond) && resultObject.CreatedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(30) && resultObject.ModifiedDate == testMS.AddMilliseconds(-testMS.Millisecond).AddMilliseconds(300));
            Assert.IsTrue(resultObject.BirthDate.Millisecond.ToString().Length == 1);
        }

        // Test combination of DateTime Json formats, to ensure behavior is consistent
        public const string Customer_HHMMSS = "{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"1900-01-01T00:00:00\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\",\"ModifiedDate\":\"1900-01-01T00:00:00\",\"Status\":0,\"BirthDate\":\"1900-01-01T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"i\",\"GenderID\":-1,\"LastName\":\"i\",\"MiddleName\":\"\"}";
        public const string Customer_HHMMSSfff = "{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-26T20:57:10.411\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\",\"ModifiedDate\":\"2017-03-26T20:57:10.411\",\"Status\":0,\"BirthDate\":\"2017-03-26T20:57:10.411\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"i\",\"GenderID\":-1,\"LastName\":\"i\",\"MiddleName\":\"\"}";
        public const string CustomerSearch_HHMMSS = "{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"1900-01-01T00:00:00\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\",\"ModifiedDate\":\"1900-01-01T00:00:00\",\"Status\":0,\"BirthDate\":\"1900-01-01T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"i\",\"GenderID\":-1,\"LastName\":\"i\",\"MiddleName\":\"\",\"Results\":[]}";
        public const string CustomerSearchResults_HHMMSS = "{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"1900-01-01T00:00:00\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\",\"ModifiedDate\":\"1900-01-01T00:00:00\",\"Status\":0,\"BirthDate\":\"1900-01-01T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"i\",\"GenderID\":-1,\"LastName\":\"i\",\"MiddleName\":\"\",\"Results\":[{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T00:48:58\",\"Id\":35,\"Key\":\"abc80489-53b3-4f6d-bdc2-135e569885c5\",\"ModifiedDate\":\"2017-03-27T00:48:58\",\"Status\":0,\"BirthDate\":\"1973-06-30T00:00:00\",\"CustomerTypeKey\":\"51a84ce1-4846-4a71-971a-cb610eeb4848\",\"FirstName\":\"Maki\",\"GenderID\":-1,\"LastName\":\"Ishii\",\"MiddleName\":\"L\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T01:25:46\",\"Id\":37,\"Key\":\"e55fe6a3-dace-4b25-b876-1c516315f687\",\"ModifiedDate\":\"2017-03-27T01:25:46\",\"Status\":0,\"BirthDate\":\"1988-03-26T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Xi\",\"GenderID\":-1,\"LastName\":\"Ling\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T01:27:12\",\"Id\":39,\"Key\":\"7fae8812-7de3-4d9f-9796-9c83dd437f80\",\"ModifiedDate\":\"2017-03-27T01:27:12\",\"Status\":0,\"BirthDate\":\"1997-03-26T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Maki\",\"GenderID\":-1,\"LastName\":\"Ishii\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T01:32:45\",\"Id\":40,\"Key\":\"58da2563-6972-49e9-9974-bb8ab8eaaf9b\",\"ModifiedDate\":\"2017-03-27T01:32:45\",\"Status\":0,\"BirthDate\":\"1943-03-26T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Maki\",\"GenderID\":-1,\"LastName\":\"Ishii\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T02:14:36\",\"Id\":42,\"Key\":\"c3e092c7-3e3b-4132-bc29-55c2ef295409\",\"ModifiedDate\":\"2017-03-27T02:14:36\",\"Status\":0,\"BirthDate\":\"1976-03-26T00:00:00\",\"CustomerTypeKey\":\"00000000-0000-0000-0000-000000000000\",\"FirstName\":\"Xi\",\"GenderID\":-1,\"LastName\":\"Ling\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T02:34:22\",\"Id\":43,\"Key\":\"8569368a-3e1d-430a-9e22-c8d03c35eb5b\",\"ModifiedDate\":\"2017-03-27T02:34:22\",\"Status\":0,\"BirthDate\":\"1932-03-26T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Xi\",\"GenderID\":-1,\"LastName\":\"Ling\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T02:34:22\",\"Id\":44,\"Key\":\"bcc2cd37-3cc2-4ae8-aa83-ddaf48a36c41\",\"ModifiedDate\":\"2017-03-27T02:34:22\",\"Status\":0,\"BirthDate\":\"1958-03-26T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Xi\",\"GenderID\":-1,\"LastName\":\"Ling\",\"MiddleName\":\"\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T15:30:18\",\"Id\":48,\"Key\":\"a447babf-159a-4799-98a0-60777d2b4d24\",\"ModifiedDate\":\"2017-03-27T15:30:18\",\"Status\":0,\"BirthDate\":\"2003-03-27T00:00:00\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Maki\",\"GenderID\":-1,\"LastName\":\"Ishii\",\"MiddleName\":\"\"}]}";
        public const string CustomerSearchResults_HHMMSSfff = "{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"1900-01-01T00:00:00.000\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\",\"ModifiedDate\":\"1900-01-01T00:00:00.000\",\"Status\":0,\"BirthDate\":\"1900-01-01T00:00:00.000\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"i\",\"GenderID\":-1,\"LastName\":\"i\",\"MiddleName\":\"\",\"Results\":[{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T00:48:58.058\",\"Id\":35,\"Key\":\"abc80489-53b3-4f6d-bdc2-135e569885c5\",\"ModifiedDate\":\"2017-03-27T00:48:58.058\",\"Status\":0,\"BirthDate\":\"1973-06-30T00:00:00.000\",\"CustomerTypeKey\":\"51a84ce1-4846-4a71-971a-cb610eeb4848\",\"FirstName\":\"Maki\",\"GenderID\":-1,\"LastName\":\"Ishii\",\"MiddleName\":\"L\"},{\"BusinessRules\":[],\"FailedRules\":[],\"CreatedDate\":\"2017-03-27T01:25:46.046\",\"Id\":37,\"Key\":\"e55fe6a3-dace-4b25-b876-1c516315f687\",\"ModifiedDate\":\"2017-03-27T01:25:46.046\",\"Status\":0,\"BirthDate\":\"1988-03-26T00:00:00.000\",\"CustomerTypeKey\":\"bf3797ee-06a5-47f2-9016-369beb21d944\",\"FirstName\":\"Xi\",\"GenderID\":-1,\"LastName\":\"Ling\",\"MiddleName\":\"\"}]}";
    }
}