//-----------------------------------------------------------------------
// <copyright file="ConfigurationValue.cs" company="GoodToCode">
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
using GoodToCode.Extensions.Data;
using GoodToCode.Extensions.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public partial class ValueConfiguration<TValue> : IValueConfiguration<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Connection String Name (key) to be used for this object's data access
        /// </summary>
        public string ConnectionName { get; set; } = ConnectionStringName.DefaultConnectionName;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string TableName { get; set; } = typeof(TValue).Name;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string ColumnPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public DataConcurrencies DataConcurrency { get; set; } = DataConcurrencies.Optimistic;

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        public DataAccessBehaviors DataAccessBehavior { get; } = DataAccessBehaviors.SelectOnly;

        /// <summary>
        /// OnModelCreating types to ignore
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(Serializer<TValue>) };

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TValue, object>>>() {
                p => p.State };

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueConfiguration() : base()
        {
            var objectWithAttributes = new TValue();
            ConnectionName = objectWithAttributes.GetAttributeValue<ConnectionStringName>(ConnectionName);
            DatabaseSchema = objectWithAttributes.GetAttributeValue<DatabaseSchemaName>(DatabaseSchema);
            TableName = objectWithAttributes.GetAttributeValue<TableName>(TableName);
            ColumnPrefix = objectWithAttributes.GetAttributeValue<ColumnPrefix>(ColumnPrefix);
            DataConcurrency = DataConcurrencyGet(DataConcurrencies.Optimistic);
            DataAccessBehavior = DataAccessBehaviorGet();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueConfiguration(string databaseSchemaName) : this()
        {
            DatabaseSchema = databaseSchemaName;
        }

        /// <summary>
        /// Concurrency model to follow in middle tier, and optionally in the data tier
        /// </summary>
        /// <returns></returns>
        private DataConcurrencies DataConcurrencyGet(DataConcurrencies defaultValue)
        {
            var returnValue = defaultValue;
            var itemType = new TValue();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataConcurrency))
                {
                    returnValue = ((DataConcurrency)item).Value;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Defines if object can select, insert, update and/or delete.
        /// </summary>
        internal DataAccessBehaviors DataAccessBehaviorGet()
        {
            var returnValue = DataAccessBehaviors.AllAccess;
            var itemType = new TValue();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataAccessBehavior))
                {
                    returnValue = ((DataAccessBehavior)item).Value;
                    break;
                }
            }

            return returnValue;
        }
    }
}
