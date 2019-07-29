//-----------------------------------------------------------------------
// <copyright file="ValidationRule.cs" company="GoodToCode">
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
using System;
using GoodToCode.Framework.Text;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// Self-validating rule based on Lambda expression
    /// </summary>
    /// <typeparam name="TEntity">Entity to validate</typeparam>
    public class ValidationRule<TEntity> : Tuple<String, Predicate<TEntity>>, IValidationRule<TEntity>
    {
        /// <summary>
        /// Property Name to validate
        /// </summary>
        public string FailMessage { get { return base.Item1; } }

        /// <summary>
        /// Expression of the validation query
        /// </summary>
        public Predicate<TEntity> Rule { get { return base.Item2; } }

        /// <summary>
        /// Type of: Errors, warnings, cant save
        /// </summary>
        public Guid ValidationRuleTypeKey { get; set; } = ValidationRuleTypes.Error;
        
        /// <summary>
        /// Is Valid
        /// </summary>
        public bool IsValid { get; private set; }
        
        /// <summary>
        /// Text message result of this business rule, with ISO code for language
        /// </summary>
        public ITextMessage Result { get; private set; } = new TextMessage();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validationQuery"></param>
        public ValidationRule(Predicate<TEntity> validationQuery)
            : base($"Rule failed: {validationQuery.ToString()}", validationQuery)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="validationQuery"></param>
        /// <param name="messageOrPropertyName"></param>
        public ValidationRule(Predicate<TEntity> validationQuery, string messageOrPropertyName)
            : base(messageOrPropertyName, validationQuery)
        { }

        /// <summary>
        /// Validates per predicate Lambda.
        /// </summary>
        /// <param name="entityToValidate"></param>
        public bool Validate(TEntity entityToValidate)
        {
            IsValid = Rule(entityToValidate);
            Result = IsValid ? new TextMessage() : new TextMessage(Item1);
            return IsValid;
        }
        
    }
}
