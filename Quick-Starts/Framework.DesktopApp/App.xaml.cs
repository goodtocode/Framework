//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Genesys Source">
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

namespace Framework.DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : WpfApplication
    {
        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public override Uri StartupUri { get; } = new Uri("/MainWindow.xaml", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public override Uri HomePage { get; } = new Uri("/MainWindow.xaml", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Error screen
        /// </summary>
        public override Uri ErrorPage { get; } = new Uri("/Pages/Shared/Error.xaml", UriKind.RelativeOrAbsolute);
    }
}
