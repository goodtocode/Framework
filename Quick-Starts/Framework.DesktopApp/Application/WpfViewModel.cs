//-----------------------------------------------------------------------
// <copyright file="ReadOnlyViewModel.cs" company="Genesys Source">
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
using Framework.Pages;
using Genesys.Framework.Application;
using Genesys.Framework.Data;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Framework.Application
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class WpfViewModel<TModel> : ViewModel<TModel> where TModel : EntityModel<TModel>, new()
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public override IApplication MyApplication { get { return (WpfApplication)System.Windows.Application.Current; } protected set { } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="webServiceControllerName">Web API support endpoint's controller to use as path</param>
        public WpfViewModel(string webServiceControllerName) 
            : base(webServiceControllerName)
        { }
        
        /// <summary>
        /// Navigates to any screen
        /// </summary>
        /// <param name="destinationUri">Destination Uri, absolute or relative</param>
        /// <param name="dataToPass">Screen data</param>
        public void Navigate(string destinationUri, object dataToPass = null)
        {
            Navigate(new Uri(destinationUri, UriKind.RelativeOrAbsolute), dataToPass);
        }

        /// <summary>
        /// Navigates to any screen
        /// </summary>
        /// <param name="destinationUri">Destination Uri, absolute or relative</param>
        /// <param name="dataToPass">Screen data</param>
        public void Navigate(Uri destinationUri, object dataToPass = null)
        {
            var navService = NavigationService.GetNavigationService(WpfApplication.Current);
            Navigate(destinationUri, dataToPass, navService);
        }

        /// <summary>
        /// Navigates to any screen
        /// </summary>
        /// <param name="destinationUri">Destination Uri, absolute or relative</param>
        /// <param name="dataToPass">Screen data</param>
        /// <param name="navService">Navigation Service object to be used to perform the .Navigate() call</param>
        public virtual void Navigate(Uri destinationUri, object dataToPass, NavigationService navService)
        {
            var newComponent = System.Windows.Application.LoadComponent(destinationUri);

            if (newComponent is ReadOnlyPage)
            {
                navService.LoadCompleted += new LoadCompletedEventHandler(((ReadOnlyPage)newComponent).NavigationService_LoadCompleted);
            }
            navService.Navigate(((Page)newComponent), dataToPass);
        }
    }
}
