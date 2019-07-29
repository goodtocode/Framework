//-----------------------------------------------------------------------
// <copyright file="ValueReader.cs" company="GoodToCode">
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
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class ValueReader<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IValueConfiguration<TValue> ConfigOptions { get; set; } = new ValueConfiguration<TValue>();

        /// <summary>
        /// Results from any query operation
        /// </summary>
        public IQueryable<TValue> Results { get; protected set; } = default(IQueryable<TValue>);

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = Defaults.Boolean;
                using (var connection = new SqlConnection(ConfigOptions.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueReader(IValueConfiguration<TValue> databaseConfig) : this()
        {
            ConfigOptions = databaseConfig;
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TValue> Read(Expression<Func<TValue, Boolean>> whereClause)
        {
            return GetByWhere(whereClause);
        }

        /// <summary>
        /// All data in this datastore subset
        ///  Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public IQueryable<TValue> GetAll()
        {
            try
            {
                Results = Data;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetAll()");
                throw;
            }

            return Results;
        }

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != Defaults.Integer And Also Key != Defaults.Guid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        public IQueryable<TValue> GetAllExcludeDefault()
        {
            try
            {
                Results = Data.Where(x => x.Key != Defaults.Guid);
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetAllExcludeDefault()");
                throw;
            }

            return Results;
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that matches by Key, or an empty entity for not found</returns>
        public TValue GetByKey(Guid key)
        {
            try
            {
                Results = Data.Where(x => x.Key == key);
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetByKey()");
                throw;
            }

            return Results.FirstOrDefaultSafe(); ;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TValue> GetByWhere(Expression<Func<TValue, Boolean>> whereClause)
        {
            try
            {
                Results = (whereClause != null) ? Data.Where<TValue>(whereClause) : Data;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetByWhere()");
                throw;
            }

            return Results;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <param name="orderByClause">Expression for order by clause</param>
        /// <param name="pageSize">Size of each result</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns></returns>
        public IQueryable<TValue> GetByPage(Expression<Func<TValue, Boolean>> whereClause, Expression<Func<TValue, Boolean>> orderByClause, int pageSize, int pageNumber)
        {
            var returnValue = default(IQueryable<TValue>);

            try
            {
                returnValue = (Data).AsQueryable();
                returnValue = (whereClause != null) ? returnValue.Where<TValue>(whereClause).AsQueryable() : returnValue;
                returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
                returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable() : returnValue;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "EntityReader.GetByPage()");
                throw;
            }

            return returnValue;
        }
    }
}
