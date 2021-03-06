//-----------------------------------------------------------------------
// <copyright file="ICustomerSearch.cs" company="GoodToCode">
//      Copyright (c) 2017-2018 GoodToCode. All rights reserved.
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
using System.Collections.Generic;

namespace Framework.Customer
{
    /// <summary>
    /// Customer
    /// </summary>
    public interface ICustomerSearch<TResult> : ICustomer where TResult : ICustomer, new()
    {
        /// <summary>
        /// Maximum number of results to return
        /// </summary>
        int MaxResults { get; set; }

        /// <summary>
        /// Search results
        /// </summary>
        List<TResult> Results { get; set; }
    }
}
