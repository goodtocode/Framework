//-----------------------------------------------------------------------
// <copyright file="IValidationRule.cs" company="GoodToCode">
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

using GoodToCode.Framework.Text;
using System;

namespace GoodToCode.Framework.Validation
{   
    /// <summary>
    /// Validation Rule contract
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IValidationRule<TEntity>
    {
        /// <summary>
        /// Rule criteria that determines pass/fail
        /// </summary>
        Predicate<TEntity> Rule { get; }

        /// <summary>
        /// Is this rule valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Validate this entity
        /// </summary>
        /// <param name="entity"></param>
        
        bool Validate(TEntity entity);

        /// <summary>
        /// Type of rule, drives database behavior
        /// </summary>
        Guid ValidationRuleTypeKey { get; }

        /// <summary>
        /// Result message
        /// </summary>
        ITextMessage Result { get; }
    }    
}
