//-----------------------------------------------------------------------
// <copyright file="ClipboardCopy.cs" company="Genesys Source">
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
using GoodToCode.Extensions;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Framework.UserControls
{
    /// <summary>
    /// Copy clipboard glyph and functionality
    /// </summary>
    public sealed partial class ClipboardCopy : UserControl
    {
        /// <summary>
        /// Text to/from clipboard
        /// </summary>
        public string Text { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClipboardCopy()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Text copied to clipboard
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            SetClipboard(Text);
            PasteButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Text retrieved from clipboard
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private async void Paste_Click(object sender, RoutedEventArgs e)
        {
            Text = await GetClipboard();
        }

        /// <summary>
        /// Sets string text to clipboard
        /// </summary>
        /// <param name="text">String to set to clipboard</param>
        public void SetClipboard(string text)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }

        /// <summary>
        /// Sets string text to clipboard
        /// </summary>
        /// <param name="text">String to set to clipboard</param>
        public async Task<string> GetClipboard()
        {
            var returnValue = Defaults.String;
            var dataPackageView = Clipboard.GetContent();

            if (dataPackageView.Contains(StandardDataFormats.Text))
                returnValue = await dataPackageView.GetTextAsync();                

            return returnValue;
        }
    }
}
