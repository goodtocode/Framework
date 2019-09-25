//-----------------------------------------------------------------------
// <copyright file="ExceptionLogTests.cs" company="GoodToCode">
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
using GoodToCode.Framework.Activity;
using System.Linq;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests code first ExceptionLog object saving activity to the database 
    /// </summary>
    [TestClass()]
    public partial class ExceptionLogTests
    {
        /// <summary>
        /// Tests code first ExceptionLog object saving activity to the database
        /// </summary>
        [TestMethod()]
        public void Core_Activity_ExceptionLog()
        {
            var preSaveCount = Defaults.Integer;
            var postSaveCount = Defaults.Integer;
            var reader = new ExceptionLogReader();
            var writer = new ExceptionLogWriter();

            preSaveCount = reader.GetAll().Count();
            var testItem = new ExceptionLog() { CustomMessage = "test" };
            testItem = writer.Save(testItem);
            postSaveCount = reader.GetAll().Count();
            Assert.IsTrue(testItem.ExceptionLogId != Defaults.Integer);
            Assert.IsTrue(postSaveCount == preSaveCount + 1);
        }
    }
}