//-----------------------------------------------------------------------
// <copyright file="INavigate.cs" company="GoodToCode">
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

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateType
    {
        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Type destinationPageType);

        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate<TModel>(Type destinationPageType, TModel dataToPass);
    }

    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateType<TModel> : INavigateType
    {

        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Type destinationPageType, TModel dataToPass);
    }
}
