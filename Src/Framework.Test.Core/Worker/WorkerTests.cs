﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Name;
using GoodToCode.Framework.Session;
using GoodToCode.Framework.Worker;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests for interop process classes
    /// </summary>
    [TestClass()]
    public class CoreProcessTests
    {
        /// <summary> 
        /// Worker_SessionContextKnownType
        /// ProcessParameter has ISessionContext as parameter, but SessionContext is passed in to DataContractJsonSerializer
        ///   Had to add SessionContext as known type
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Worker_SessionContextKnownType()
        {
            String dataToSendSerialized2 = string.Empty;
            SessionContext context = new SessionContext(this.ToString(), Guid.NewGuid().ToString(), "MyName");
            NameIdDto dataIn = new NameIdDto() { Name = "NameField" };
            WorkerParameter<NameIdDto> item2 = new WorkerParameter<NameIdDto>() { Context = context, DataIn = dataIn };
            ISerializer<WorkerParameter<NameIdDto>> serializer2 = new JsonSerializer<WorkerParameter<NameIdDto>>();

            // Test Serialization            
            dataToSendSerialized2 = serializer2.Serialize(item2);
            Assert.IsTrue(dataToSendSerialized2 != string.Empty, "Did not work");
        }

        /// <summary> 
        /// Worker_WorkerParameterSerialize
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Worker_WorkerParameterSerialize()
        {
            // Initialize
            var dataToSendSerialized = string.Empty;
            var context = new SessionContext(this.ToString(), Guid.NewGuid().ToString(), "MyName");
            var dataIn = new NameIdDto() { Name = "NameField" };
            var item1 = new WorkerParameter<NameIdDto>() { Context = context, DataIn = dataIn };
            var serializer = new JsonSerializer<WorkerParameter<NameIdDto>>();

            // Disable exceptions, we just want to look at results
            serializer.ThrowException = false;

            // Test Item1 Serialization
            dataToSendSerialized = serializer.Serialize(item1);
            Assert.IsTrue(dataToSendSerialized != string.Empty, "Did not work");
            item1 = serializer.Deserialize(dataToSendSerialized);
            Assert.IsTrue(item1 != null, "Did not work.");
        }

        /// <summary>
        /// Person_PersonTests
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Core_Worker_WorkerResultSerialize()
        {
            // Initialize
            var DataToSendSerialized = string.Empty;
            var Item1 = new SessionContext(this.ToString(), Guid.NewGuid().ToString(), "MyName");
            ISerializer<ISessionContext> Serializer1 = new JsonSerializer<ISessionContext>();
            ISerializer<SessionContext> Serializer2 = new JsonSerializer<SessionContext>();

            // Test Serialization
            DataToSendSerialized = Serializer1.Serialize(Item1);
            Assert.IsTrue(DataToSendSerialized != string.Empty, "Did not work");

            // Test Serialization
            DataToSendSerialized = Serializer2.Serialize(Item1);
            Assert.IsTrue(DataToSendSerialized != string.Empty, "Did not work");
        }
    }
}