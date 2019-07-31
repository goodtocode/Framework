//-----------------------------------------------------------------------
// <copyright file="IGetOperationAsync.cs" company="GoodToCode">
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
using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Read operations against an Async datastore, such as Http resource server
    /// Both Id and Key can be used as 1-1 unique idenfiers
    ///  For Internal, high-performance, multi-join lookups: int Id
    ///  For External, low-volume tables, obfuscated, guaranteed-unique: Guid Key
    /// </summary>
    /// <typeparam name="TEntity">Entity type to be read</typeparam>
    public interface IGetOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets all items from the datastore
        /// Expects additional constraints to be attached by the caller
        ///  Usage: customer.GetAll().Where(x => x.FirstName == "Jo")
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != Defaults.Integer And Also Key != Defaults.Guid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllExcludeDefaultAsync();

        /// <summary>
        /// Gets one or no items based on exact Id match
        /// </summary>
        /// <returns>One or no TEntity based on exact Id match</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets one or no items based on exact Key match
        /// </summary>
        /// <returns>One or no TEntity based on exact Key match</returns>
        Task<TEntity> GetByKeyAsync(Guid key);
    }
}
