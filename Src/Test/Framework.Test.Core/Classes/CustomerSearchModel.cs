//-----------------------------------------------------------------------
// <copyright file="CustomerSearchModel.cs" company="GoodToCode">
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
using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Customer Search Results
    /// </summary>    
    public class CustomerSearchModel : EntityModel<CustomerSearchModel>
    {
        private int maxResults = 25;

        /// <summary>
        /// FirstName of customers
        /// </summary>     
        public string FirstName { get; set; } = Defaults.String;

        /// <summary>
        /// MiddleName of customer
        /// </summary>
        public string MiddleName { get; set; } = Defaults.String;

        /// <summary>
        /// LastName of customer
        /// </summary>
        public string LastName { get; set; } = Defaults.String;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public DateTime BirthDate { get; set; } = Defaults.Date;

        /// <summary>
        /// Gender of customer
        /// </summary>
        public int GenderId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Type of customer
        /// </summary>
        public Guid CustomerTypeKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Search results
        /// </summary>
        public List<CustomerModel> Results { get; set; } = new List<CustomerModel>();

        /// <summary>
        /// Maximum number of results to return
        ///  Will not accept negative number, flips back to default (25)
        /// </summary>
        public int MaxResults
        {
            get => maxResults;
            set => maxResults = value > 0 ? value : maxResults;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public CustomerSearchModel()
                : base()
        {
        }
    }
}