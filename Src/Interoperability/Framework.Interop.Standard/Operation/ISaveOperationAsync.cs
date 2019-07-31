//-----------------------------------------------------------------------
// <copyright file="ISaveOperationAsync.cs" company="GoodToCode">
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
using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface ISaveOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object updated and all current values as of the save</returns>
        Task<TEntity> SaveAsync(TEntity entity);
    }
}
