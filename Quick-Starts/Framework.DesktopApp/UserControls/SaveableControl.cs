//-----------------------------------------------------------------------
// <copyright file="SaveableControl.cs" company="Genesys Source">
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
using Genesys.Framework.Worker;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics.CodeAnalysis;

namespace Framework.UserControls
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>
    public abstract class SaveableControl : ReadOnlyControl
    {
        /// <summary>
        /// OK cancel buttons for this processing screen
        /// </summary>
        protected abstract OkCancel OkCancel { get; set; } //ToDo: Fix

        /// <summary>
        /// Result of the screens process
        /// </summary>
        public WorkerResult Result { get; private set; } = new WorkerResult();

        /// <summary>
        /// Constructor
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaveableControl() : base()
        {
            OkCancel.OnOKClicked += OK_Click;
            OkCancel.OnCancelClicked += Cancel_Click;
        }
       
        
        /// <summary>
        /// OK button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected async void OK_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.StartProcessing();
            Result = await Process();
            OkCancel.StopProcessing(Result);
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected void Cancel_Click(object sender, RoutedEventArgs e)
        {
            OkCancel.CancelProcessing();
            Cancel();
        }
        
        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract Task<WorkerResult> Process();

        /// <summary>
        /// Processes a forms business function
        /// </summary>
        public abstract void Cancel();
    }
}
