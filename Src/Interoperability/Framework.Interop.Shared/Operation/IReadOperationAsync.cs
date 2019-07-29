//-----------------------------------------------------------------------
// <copyright file="IReadOperation.cs" company="GoodToCode">
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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface IReadOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Retrieve TEntity objects operations
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        Task<IList<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Retrieve TEntity objects operations
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        Task<TEntity> ReadSingleAsync(Expression<Func<TEntity, bool>> expression);
    }
}
