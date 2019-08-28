//-----------------------------------------------------------------------
// <copyright file="IConfigurationValue.cs" company="GoodToCode">
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
using GoodToCode.Extras.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Database connection and metadata info
    /// </summary>
    public partial interface IValueConfiguration<TValue> : IEntityTypeConfiguration<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        string DatabaseSchema { get; set; }

        /// <summary>
        /// Connection String Name (key) to be used for this object's data access
        /// </summary>
        string ConnectionName { get; set; }

        /// <summary>
        /// Connection String Name (key) to be used for this object's data access
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Table this configuration will interact
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Prefix for all table columns. 
        ///  Default/No prefix: Property names must  match the column like Id, Key, FirstName 
        ///  Prefix provided is applied to all: CustomerId, CustomerKey, CustomerFirstName
        /// </summary>
        string ColumnPrefix { get; set; }

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        DataConcurrencies DataConcurrency { get; set; }

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        DataAccessBehaviors DataAccessBehavior { get; }

        /// <summary>
        /// List of types to ignore in database operations
        /// </summary>
        IList<Type> IgnoredTypes { get; }

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
    }
}
