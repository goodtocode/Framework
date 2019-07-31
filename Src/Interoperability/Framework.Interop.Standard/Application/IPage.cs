//-----------------------------------------------------------------------
// <copyright file="IPage.cs" company="GoodToCode">
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
using GoodToCode.Extras.Net;
using System;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>    
    interface IPage
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        IApplication MyApplication { get; }

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        HttpVerbSender HttpSender { get; set; }

        /// <summary>
        /// Name of the controller used in web service calls
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// Uri to currently active frame/page
        /// </summary>
        Uri CurrentPage { get; }

        /// <summary>
        /// Throws Exception if any UI elements overrun their text max length
        /// </summary>
        bool ThrowExceptionOnTextOverrun { get; set; }
                
        /// <summary>
        /// Binds all model data to the screen controls and sets MyViewModel.MyModel property
        /// </summary>
        /// <param name="modelData">Model data to bind</param>
        void BindModel(object modelData);

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        void Page_Loaded(object sender, EventArgs e);      
    }
}
