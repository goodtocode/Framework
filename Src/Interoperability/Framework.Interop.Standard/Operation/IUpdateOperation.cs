//-----------------------------------------------------------------------
// <copyright file="IUpdateOperation.cs" company="GoodToCode">
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

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Update operations
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface IUpdateOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Update operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object pulled from datastore</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <param name="entity">Entity to be updated in the datastore</param>
        /// <returns>True if rules and setup allow for update, else false</returns>
        bool CanUpdate(TEntity entity);
    }
}
