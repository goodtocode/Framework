//-----------------------------------------------------------------------
// <copyright file="ActiveRecordEntity.cs" company="GoodToCode">
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
using GoodToCode.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  ActiveRecord abstract class. Based on StoredProcedureEntity
    ///    Includes GetById(), GetByKey(), GetByWhere(), GetAll(), Save(), Delete()
    /// </summary>
    public abstract class ActiveRecordEntity<TEntity> : StoredProcedureEntity<TEntity> where TEntity : StoredProcedureEntity<TEntity>, new()
    {
        /// <summary>
        /// Gets single record by Id
        /// </summary>
        /// <param name="id">Integer Id that is the primary key or a unique identity</param>
        /// <returns></returns>
        public static TEntity GetById(int id)
        {
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetById(id);
            return returnValue;
        }

        /// <summary>
        /// Gets single record by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TEntity GetByKey(Guid key)
        {
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetByKey(key);
            return returnValue;
        }

        /// <summary>
        /// Generic read functionality
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetAll()
        {
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetAll();
            return returnValue;
        }

        /// <summary>
        /// Gets by an expression for Where clause
        /// </summary>
        /// <param name="expression">Expression for where clause, typically to filter</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> expression)
        {
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetByWhere(expression);
            return returnValue;
        }

        /// <summary>
        /// Inserts and Updates to database
        /// </summary>
        /// <returns></returns>
        public TEntity Save()
        {
            var writer = new StoredProcedureWriter<TEntity>();
            return writer.Save(this.CastOrFill<TEntity>());
        }

        /// <summary>
        /// Inserts and Updates to database
        /// </summary>
        /// <param name="activity">Activity responsible for tracking this process</param>        
        public TEntity Save(IActivityContext activity)
        {
            ActivityContextKey = activity.ActivityContextKey;
            return Save();
        }

        /// <summary>
        /// Delete from the datastore
        /// </summary>
        public TEntity Delete()
        {
            var writer = new StoredProcedureWriter<TEntity>();
            return writer.Delete(this.CastOrFill<TEntity>());
        }

        /// <summary>
        /// Delete from the datastore
        /// </summary>
        public TEntity Delete(IActivityContext activity)
        {
            ActivityContextKey = activity.ActivityContextKey;
            return Delete();
        }
    }
}
