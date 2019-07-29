//-----------------------------------------------------------------------
// <copyright file="UniversalViewModel.cs" company="Genesys Source">
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
using Framework.Themes;
using Genesys.Framework.Application;
using Genesys.Framework.Data;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class UniversalViewModel<TModel> : ViewModel<TModel>, INavigateType<TModel> where TModel : EntityModel<TModel>, new()
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public override IApplication MyApplication { get { return (UniversalApplication)Windows.UI.Xaml.Application.Current; } protected set { } }

        /// <summary>
        /// Returns currently active window
        /// </summary>
        public static Window Current
        {
            get
            {
                return Window.Current;
            }
        }

        /// <summary>
        /// Gets the root frame of the application
        /// </summary>
        /// <returns></returns>
        public Frame RootFrame
        {
            get
            {
                var returnValue = new Frame();
                var masterLayout = new GenericLayout();
                if (Current.Content is GenericLayout)
                {
                    masterLayout = (GenericLayout)Current.Content;
                    returnValue = masterLayout.ContentFrame;
                } else if (Current.Content is Frame)
                {
                    returnValue = (Frame)Current.Content;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webServiceControllerName"></param>
        public UniversalViewModel(string webServiceControllerName) 
            : base(webServiceControllerName)
        { }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destinationPageType">Destination page Uri</param>
        public bool Navigate(Type destinationPageType) { return this.Navigate(destinationPageType); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destinationPageType">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate(Type destinationPageType, TModel dataToPass) { return RootFrame.Navigate(destinationPageType, dataToPass); }

        /// <summary>
        /// Navigates to a page via type.
        /// Typically in Universal apps
        /// </summary>
        /// <param name="destinationPageType">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<T>(Type destinationPageType, T dataToPass) { return RootFrame.Navigate(destinationPageType, dataToPass); }
    }
}
