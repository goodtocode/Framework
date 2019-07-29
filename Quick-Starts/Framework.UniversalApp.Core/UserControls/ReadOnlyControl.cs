//-----------------------------------------------------------------------
// <copyright file="ReadOnlyControl.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
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
using Framework.Application;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.UserControls
{
    /// <summary>
    /// Common UI functions
    /// </summary>
    public abstract class ReadOnlyControl : UserControl
    {
        /// <summary>
        /// MyApplication instance
        /// </summary>
        public UniversalApplication MyApplication { get { return (UniversalApplication)Windows.UI.Xaml.Application.Current; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ReadOnlyControl() : base()
        {
            Loaded += Partial_Loaded;
            SizeChanged += Partial_SizeChanged;
        }

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected virtual void Partial_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Page_SizeChanged event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void Partial_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
        
        /// <summary>
        /// Binds all model data to the screen controls
        /// </summary>
        /// <param name="modelData">Data to bind to controls</param>
        protected abstract void BindModelData(object modelData);

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void SendBeginEventHandler(object sender, EventArgs e);
    }
}