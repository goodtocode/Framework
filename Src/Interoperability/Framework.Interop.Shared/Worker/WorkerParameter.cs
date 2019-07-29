//-----------------------------------------------------------------------
// <copyright file="WorkerParameter.cs" company="GoodToCode">
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
using GoodToCode.Framework.Security;
using GoodToCode.Framework.Session;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    /// <typeparam name="TDataIn">Type of data to pass</typeparam>
    public class WorkerParameter<TDataIn> : IWorkerParameter<TDataIn>
    {
        private SessionContext context = new SessionContext();
        
        /// <summary>
        /// Identity of user initiating this process
        /// </summary>
        public SessionContext Context { get { return context; } set { context = value as SessionContext; } }

        // Insist any interface types have a concrete equivalent, especially for serialization
        ISessionContext IWorkerParameter<TDataIn>.Context { get { return context; } set { context = value as SessionContext; } }

        /// <summary>
        /// Data to be returned
        /// </summary>
        public TDataIn DataIn { get; set; } = TypeExtension.InvokeConstructorOrDefault<TDataIn>();
        
        /// <summary>
        /// Force hydration on constructor
        /// </summary>
        public WorkerParameter() : base()
        {
        }

        /// <summary>
        /// Constructor that partially hydrates
        /// </summary>
        public WorkerParameter(TDataIn inputData) : this()
        {
            DataIn = inputData;
        }

        /// <summary>
        /// Constructor that fully hydrates
        /// </summary>
        public WorkerParameter(UserPrincipal principalIdentity, TDataIn inputData) : this(inputData)
        {
            Context.Fill(principalIdentity);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">User, device and app context</param>
        /// <param name="data">Data to send</param>
        public WorkerParameter(ISessionContext context, TDataIn data) : this(data)
        {
            Context.Fill(context);
        }       
    }
}
