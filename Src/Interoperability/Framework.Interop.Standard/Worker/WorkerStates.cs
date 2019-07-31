//-----------------------------------------------------------------------
// <copyright file="WorkerStates.cs" company="GoodToCode">
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

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// States of a an operation that does work
    /// </summary>
    public enum WorkerStates
    {
        /// <summary>
        /// Process never executed
        /// </summary>
        NeverRan = 0x0,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        Pending = 0x1,

        /// <summary>
        /// Process is currently running
        /// </summary>
        Running = 0x2,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        OnHold = 0x4,

        /// <summary>
        /// Process completed with no errors
        /// </summary>
        Completed = 0x8,
    }
}
