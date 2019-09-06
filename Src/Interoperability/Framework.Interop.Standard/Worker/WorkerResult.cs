//-----------------------------------------------------------------------
// <copyright file="WorkerResult.cs" company="GoodToCode">
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
using GoodToCode.Extensions;
using GoodToCode.Extensions.Collections;
using GoodToCode.Framework.Validation;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    public class WorkerResult : IWorkerResult
    {      
        /// <summary>
        /// Current state of the process
        /// </summary>
        public WorkerStates CurrentState { get; set; } = WorkerStates.NeverRan;

        /// <summary>
        /// Errors
        /// </summary>
        /// <value></value>        
        public KeyValueListString FailedRules { get; set; } = new KeyValueListString();

        /// <summary>
        /// Id to be returned to caller
        /// </summary>
        public int ReturnId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Key to be returned to caller
        /// </summary>
        public Guid ReturnKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Serialized data to be returned to caller
        /// </summary>
        public string ReturnData { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkerResult() : base()
        {
        }
        
        /// <summary>
        /// Adds to failed rules from a valid IValidationRule or IValidationResult
        /// </summary>
        /// <param name="validatable"></param>
        public void RuleFailed(ValidationResult validatable)
        {
            FailedRules.Add(validatable);
        }

        /// <summary>
        /// Adds a failed rule message that does not have access to a full IValidationRule or IValidationResult
        /// </summary>
        /// <param name="resultMessageWithNoValidationRule"></param>
        public void RuleFailed(string resultMessageWithNoValidationRule)
        {
            FailedRules.Add(new ValidationResult(resultMessageWithNoValidationRule));
        }
        
        /// <summary>
        /// Starts a process
        /// </summary>        
        public void OnStart()
        {
            CurrentState = WorkerStates.Running;
        }

        /// <summary>
        /// Records failure of a process
        /// </summary>
        public void OnFail(string errorMessage)
        {
            CurrentState = WorkerStates.OnHold;
            RuleFailed(errorMessage);
        }

        /// <summary>
        /// Finalizes a process
        /// </summary>
        public void OnSuccess()
        {
            CurrentState = WorkerStates.Completed;
        }
    }
}
