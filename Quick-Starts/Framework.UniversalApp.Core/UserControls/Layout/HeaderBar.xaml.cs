//-----------------------------------------------------------------------
// <copyright file="HeaderBar.cs" company="Genesys Source">
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
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Framework.UserControls
{
    /// <summary>
    /// Top bar with title and back button
    /// </summary>
    
    public sealed partial class HeaderBar : ReadOnlyControl
    {
        /// <summary>
        /// Manages text of the header
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public string Text
        {
            get { return TextPageHeader.Text; }
            set { TextPageHeader.Text = value; }
        }

        /// <summary>
        /// TextBlock that displays Text property
        /// </summary>
        public TextBlock TextControl
        {
            get { return TextPageHeader; }
            set { TextPageHeader = value; }
        }

        /// <summary>
        /// Allows setting of the text foreground
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public new Brush Foreground
        {
            get { return TextPageHeader.Foreground; }
            set { TextPageHeader.Foreground = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HeaderBar()
        {
            InitializeComponent();
            GotFocus += TopBarControl_GotFocus;
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
        /// Configures self based on RootFrame.CanGoBack
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Event args</param>
        private void TopBarControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (MyApplication.RootFrame.CanGoBack == false)
            {
                ButtonGoBack.IsEnabled = false;
            } else
            {
                ButtonGoBack.IsEnabled = true;
                ButtonGoBack.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            if (MyApplication.RootFrame.CanGoBack)
            {
                MyApplication.RootFrame.GoBack();
            }
        }
    }

}