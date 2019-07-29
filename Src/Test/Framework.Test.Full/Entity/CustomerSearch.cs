//-----------------------------------------------------------------------
// <copyright file="CustomerSearch.cs" company="GoodToCode">
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
using GoodToCode.Extensions;
using System;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Simulates a customer business object search class, for passing over Http and binding to screens
    /// </summary>
    public class CustomerSearch
    {
        public int Id { get; set; } = Defaults.Integer;
        public Guid Key { get; set; } = Defaults.Guid;
        public string FirstName { get; set; } = Defaults.String;
        public string MiddleName { get; set; } = Defaults.String;
        public string LastName { get; set; } = Defaults.String;
        public DateTime BirthDate { get; set; } = Defaults.Date;
        public int GenderId { get; set; } = Defaults.Integer;
        public Guid CustomerTypeKey { get; set; } = Defaults.Guid;
        public DateTime CreatedDate { get; set; } = Defaults.Date;
        public DateTime ModifiedDate { get; set; } = Defaults.Date;
        public int ActivityContextId { get; set; } = Defaults.Integer;
        public List<CustomerInfo> Results { get; set; } = new List<CustomerInfo>();
        public CustomerSearch()
                : base()
        {
        }
    }
}
