//-----------------------------------------------------------------------
// <copyright file="OkCancel.cs" company="Genesys Source">
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
using System.Diagnostics.CodeAnalysis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.UserControls
{
    /// <summary>
    /// OK and cancel buttons
    /// </summary>
    public sealed partial class OkCancel : ReadOnlyControl
    {
        /// <summary>
        /// OnOKClickedEventHandler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void OnOKClickedEventHandler(object sender, RoutedEventArgs e);

        /// <summary>
        /// OnOKClickedEventHandler
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event OnOKClickedEventHandler OnOKClicked;

        /// <summary>
        /// OnCancelClickedEventHandler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void OnCancelClickedEventHandler(object sender, RoutedEventArgs e);

        /// <summary>
        /// OnCancelClickedEventHandler
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event OnCancelClickedEventHandler OnCancelClicked;

        /// <summary>
        /// Shows/hides the map
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public Visibility VisibilityButtons
        {
            get
            {
                return ButtonOK.Visibility;
            }
            set
            {
                ButtonOKControl.Visibility = value;
                ButtonCancelControl.Visibility = value;
            }
        }

        /// <summary>
        /// HorizontalAlignment
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public new HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return StackButtons.HorizontalAlignment;
            }
            set
            {
                StackButtons.HorizontalAlignment = value;
            }
        }

        /// <summary>
        /// Orientation
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public Orientation Orientation
        {
            get
            {
                return StackButtons.Orientation;
            }
            set
            {
                StackButtons.Orientation = value;
            }
        }

        /// <summary>
        /// Progress ring text 
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextProcessingMessage
        {
            get
            {
                return ProgressProcessing.TextProcessingMessage;
            }
            set
            {
                ProgressProcessing.TextProcessingMessage = value;
            }
        }

        /// <summary>
        /// Progress result
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string TextResultMessage
        {
            get
            {
                return ProgressProcessing.TextResultMessage;
            }
            set
            {
                ProgressProcessing.TextResultMessage = value;
            }
        }

        /// <summary>
        /// OK Button
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public Button ButtonOK
        {
            get
            {
                return ButtonOKControl;
            }
            set
            {
                ButtonOKControl = value;
            }
        }

        /// <summary>
        /// OK Button content
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public object ButtonOKContent
        {
            get
            {
                return ButtonOKControl.Content;
            }
            set
            {
                ButtonOKControl.Content = value;
            }
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public Button ButtonCancel
        {
            get
            {
                return ButtonCancelControl;
            }
            set
            {
                ButtonCancelControl = value;
            }
        }

        /// <summary>
        /// OK Button content
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public object ButtonCancelContent
        {
            get
            {
                return ButtonCancelControl.Content;
            }
            set
            {
                ButtonCancelControl.Content = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OkCancel()
        {
            InitializeComponent();
            ButtonOKControl.Click += OK_Click;
            ButtonCancelControl.Click += Cancel_Click;
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="modelData"></param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, RoutedEventArgs e)
        {
            base.Partial_Loaded(sender, e);
        }

        /// <summary>
        /// Ok button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (OnOKClicked != null)
                OnOKClicked(sender, e);
        }

        /// <summary>
        /// Cancel Button
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelClicked != null)
                OnCancelClicked(sender, e);
        }

        /// <summary>
        /// Starts the processing
        /// </summary>
        public void StartProcessing()
        {
            StackButtons.Visibility = Visibility.Collapsed;
            ProgressProcessing.StartProcessing();
        }

        /// <summary>
        /// Starts the processing
        /// </summary>
        public void StartProcessing(string processingMessage)
        {
            StackButtons.Visibility = Visibility.Collapsed;
            ProgressProcessing.StartProcessing(processingMessage);
        }

        /// <summary>
        /// Cancels processing with no message and no display of processing results.
        /// </summary>
        public void CancelProcessing()
        {
            ProgressProcessing.CancelProcessing();
            StackButtons.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Stops processing, and supplies WorkerResult data
        /// </summary>
        /// <param name="results">WorkerResult class with results of the processing.</param>
        public void StopProcessing(string successMessage)
        {
            ProgressProcessing.StopProcessing(new WorkerResult(), successMessage);
            StackButtons.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Stops processing, and supplies WorkerResult data
        /// </summary>
        /// <param name="results">WorkerResult class with results of the processing.</param>
        /// <param name="successMessage">Message to display if success</param>
        public void StopProcessing(WorkerResult results)
        {
            ProgressProcessing.StopProcessing(results);
            StackButtons.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Stops processing, and supplies WorkerResult data
        /// </summary>
        /// <param name="results">WorkerResult class with results of the processing.</param>
        /// <param name="successMessage">Message to display if success</param>
        public void StopProcessing(WorkerResult results, string successMessage)
        {
            ProgressProcessing.StopProcessing(results, successMessage);
            StackButtons.Visibility = Visibility.Visible;
        }
    }
}