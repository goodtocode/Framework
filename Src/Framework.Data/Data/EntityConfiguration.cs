//-----------------------------------------------------------------------
// <copyright file="ConfigurationBuilder.cs" company="GoodToCode">
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
using GoodToCode.Extensions.Collections;
using GoodToCode.Extensions.Data;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public partial class EntityConfiguration<TEntity> : IEntityConfiguration<TEntity> where TEntity : EntityInfo<TEntity>, new()
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
        public string TableName { get; set; } = typeof(TEntity).Name;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string ColumnPrefix { get; set; } = Defaults.String;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public DataConcurrencies DataConcurrency { get; set; } = DataConcurrencies.Optimistic;

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        public DataAccessBehaviors DataAccessBehavior { get; set; } = DataAccessBehaviors.AllAccess;

        /// <summary>
        /// Number of rows affected by any operation
        /// </summary>
        public int RowsAffected { get; set; } = Defaults.Integer;

        /// <summary>
        /// OnModelCreating types to ignore
        /// Begins initialized with common framework utility classes
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(EntityValidator<TEntity>),
                typeof(ValidationRule<TEntity>),
                typeof(Serializer<TEntity>),
                typeof(StoredProcedure<TEntity>),
                typeof(KeyValueListString)};

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TEntity, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TEntity, object>>>()
            {
                p => p.State,
                p => p.FailedRules
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration() : base()
        {
            var objectWithAttributes = new TEntity();
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
        public EntityConfiguration(string databaseSchemaName) : this()
        {
            DatabaseSchema = databaseSchemaName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration(Expression<Func<TEntity, object>> ignoredProperty) : this()
        {
            IgnoredProperties.Add(ignoredProperty);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration(IList<Expression<Func<TEntity, object>>> ignoredProperties) : this()
        {
            IgnoredProperties.AddRange(ignoredProperties);
        }

        /// <summary>
        /// Concurrency model to follow in middle tier, and optionally in the data tier
        /// </summary>
        /// <returns></returns>
        private DataConcurrencies DataConcurrencyGet(DataConcurrencies defaultValue)
        {
            var returnValue = defaultValue;
            var itemType = new TEntity();

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
            var itemType = new TEntity();

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
