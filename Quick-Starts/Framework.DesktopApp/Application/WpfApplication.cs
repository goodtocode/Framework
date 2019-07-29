//-----------------------------------------------------------------------
// <copyright file="WpfApplication.cs" company="Genesys Source">
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
using System.Threading.Tasks;
using System.Windows.Controls;
using Genesys.Extensions;
using Genesys.Extras.Configuration;
using Genesys.Extras.Net;
using System.Windows;
using System.Linq;
using System.Windows.Navigation;

namespace Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public abstract class WpfApplication : System.Windows.Application, IWpfApplication
    {
        /// <summary>
        /// Persistent ConfigurationManager class, automatically loaded with this project .config files
        /// </summary>
        public IConfigurationManager ConfigurationManager { get; set; } = new ConfigurationManagerFull(ApplicationTypes.Native);

        /// <summary>
        /// MyWebService
        /// </summary>
        public Uri MyWebService { get { return new Uri(ConfigurationManager.AppSettingValue("MyWebService"), UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public abstract new Uri StartupUri { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public abstract Uri HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        public abstract Uri ErrorPage { get; }

        /// <summary>
        /// Returns currently active window
        /// </summary>
        public static new Window Current { get { return System.Windows.Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive); } }

        /// <summary>
        /// Current loaded page
        /// </summary>
        public Uri CurrentPage { get { return ((NavigationWindow)WpfApplication.Current).CurrentSource; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public WpfApplication() : base()
        {
            Startup += OnStartup;
            OnObjectInitialize();
        }

        /// <summary>
        /// Loads config data
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            await Task.Delay(1);
            ConfigurationManager = new ConfigurationManagerFull(ApplicationTypes.Native);
        }

        /// <summary>
        /// Wakes up any sleeping processes, and MyWebService chain
        /// </summary>
        /// <returns></returns>
        public virtual async Task WakeServicesAsync()
        {
            if (MyWebService.ToString() == Defaults.String)
            {
                HttpRequestGetString Request = new HttpRequestGetString(MyWebService.ToString())
                {
                    ThrowExceptionWithEmptyReponse = false
                };
                await Request.SendAsync();
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
                Frame returnValue = new Frame();

                if (Current.Content is Frame)
                {
                    returnValue = (Frame)Current.Content;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get { return RootFrame.CanGoBack; } }

        /// <summary>
        /// Can this screen go forward
        /// </summary>
        public bool CanGoForward { get { return RootFrame.CanGoForward; } }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoHome() { Navigate(HomePage); }

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { if(CanGoBack) RootFrame.GoBack(); }

        /// <summary>
        /// Navigates forward to next screen
        /// </summary>
        public void GoForward() { if (CanGoForward) RootFrame.GoForward(); }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        public bool Navigate(Uri destinationPageUrl) { return RootFrame.Navigate(destinationPageUrl); }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<TModel>(Uri destinationPageUrl, TModel dataToPass) { return RootFrame.Navigate(destinationPageUrl, dataToPass); }

        /// <summary>
        /// Ensure StartupUri is set during Startup event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStartup(object sender, EventArgs e)
        {
            base.StartupUri = StartupUri;
        }

        /// <summary>
        /// New model to load
        /// </summary>
        public event ObjectInitializeEventHandler ObjectInitialize;

        /// <summary>
        /// OnObjectInitialize()
        /// </summary>
        protected async void OnObjectInitialize()
        {
            await this.LoadDataAsync();
            await this.WakeServicesAsync();
            ObjectInitialize?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void ObjectInitializeEventHandler(object sender, EventArgs e);        
        }
}
