﻿//-----------------------------------------------------------------------
// <copyright file="IUniversalApplication.cs" company="GoodToCode">
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
using GoodToCode.Framework.Application;
using System;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface IUniversalApplication : IApplication, INavigateType
    {
        /// <summary>
        /// Currently active page Uri
        /// </summary>
        Type CurrentPage { get; }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        Type StartupPage { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        Type HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        Type ErrorPage { get; }        
    }
}