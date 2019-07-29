//-----------------------------------------------------------------------
// <copyright file="GenericLayout.cs" company="GoodToCode">
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
using System;
using System.Windows;
using System.Windows.Controls;

namespace GoodToCode.Framework.Themes
{
    /// <summary>
    /// Default layout for Generic theme
    /// </summary>
	public class GenericLayout : Control
    {
        /// <summary>
        /// Constructor
        /// </summary>
		static GenericLayout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GenericLayout), new FrameworkPropertyMetadata(typeof(GenericLayout)));
        }

        /// <summary>
        /// Title
        /// </summary>
		public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Title Property
        /// </summary>
		public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(GenericLayout), new UIPropertyMetadata());

        /// <summary>
        /// Content Pane Header
        /// </summary>
		public object ContentHeader
        {
            get { return (object)GetValue(ContentHeaderProperty); }
            set { SetValue(ContentHeaderProperty, value); }
        }
        /// <summary>
        /// ContentHeader Property
        /// </summary>
		public static readonly DependencyProperty ContentHeaderProperty =
            DependencyProperty.Register("ContentHeader", typeof(object), typeof(GenericLayout), new UIPropertyMetadata());

        /// <summary>
        /// Main content pane body
        /// </summary>
		public object ContentBody
        {
            get { return (object)GetValue(ContentBodyProperty); }
            set { SetValue(ContentBodyProperty, value); }
        }

        /// <summary>
        /// ContentBody Property
        /// </summary>
		public static readonly DependencyProperty ContentBodyProperty =
            DependencyProperty.Register("ContentBody", typeof(object), typeof(GenericLayout), new UIPropertyMetadata());
    }
}
