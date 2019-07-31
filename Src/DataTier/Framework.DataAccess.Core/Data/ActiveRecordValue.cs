//-----------------------------------------------------------------------
// <copyright file="ActiveRecordValue.cs" company="GoodToCode">
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
using GoodToCode.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  ActiveRecord abstract class. Based on StoredProcedureValue
    ///    Includes GetById(), GetByKey(), GetByWhere(), GetAll(), Save(), Delete()
    /// </summary>
    public abstract class ActiveRecordValue<TValue> : ValueInfo<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Gets single record by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetByKey(Guid key)
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetByKey(key);
            return returnValue;
        }

        /// <summary>
        /// Generic read functionality
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TValue> GetAll()
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetAll();
            return returnValue;
        }

        /// <summary>
        /// Gets by an expression for Where clause
        /// </summary>
        /// <param name="expression">Expression for where clause, typically to filter</param>
        /// <returns></returns>
        public static IEnumerable<TValue> GetByWhere(Expression<Func<TValue, bool>> expression)
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetByWhere(expression);
            return returnValue;
        }
    }
}
