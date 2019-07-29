//-----------------------------------------------------------------------
// <copyright file="HamburgerMenu.cs" company="Genesys Source">
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
using Genesys.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace Framework.UserControls
{
    /// <summary>
    /// Interaction logic for HamburgerMenu.xaml
    /// </summary>
    public partial class HamburgerMenu : ReadOnlyControl
    {
        private bool isOpen = Defaults.Boolean;

        /// <summary>
        /// Current state of the menu, open or closed
        /// </summary>
        public bool IsOpen { get { return isOpen; } }

        /// <summary>
        /// Open/Close animation duration
        /// </summary>
        public double AnimationDuration { get; set; } = 0.5;

        /// <summary>
        /// Width of menu bar when closed
        /// </summary>
        public int WidthClosed { get; set; } = 20;

        /// <summary>
        /// Width of menu bar when open
        /// </summary>
        public int WidthOpen { get; set; } = 200;

        /// <summary>
        /// Constructor
        /// </summary>
        public HamburgerMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Binds controls to the data 
        /// </summary>
        /// <typeparam name="TModel">Model of this page</typeparam>
        /// <param name="modelData">Data to bind to page</param>
        protected override void BindModelData(object modelData)
        {
        }

        /// <summary>
        /// Partial and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Partial_Loaded(object sender, EventArgs e)
        {
            base.Partial_Loaded(sender, e);
        }

        /// <summary>
        /// Handles menu clicks to open or close the menu
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Menu_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.IsOpen)
            {
                MenuClose(sender, e);
            } else
            {
                MenuOpen(sender, e);
            }
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Navigate(MyApplication.HomePage);
        }

        /// <summary>
        /// Navigates to CustomerSummary screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Navigate(CustomerSearch.Uri);
        }

        /// <summary>
        /// Navigates to CustomerCreate screen, passing a test Customer to bind and display
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            var newComponent = System.Windows.Application.LoadComponent(CustomerCreate.Uri);
            var navService = NavigationService.GetNavigationService(this);

            if (newComponent is ReadOnlyPage)
            {
                navService.LoadCompleted += new LoadCompletedEventHandler(((ReadOnlyPage)newComponent).NavigationService_LoadCompleted);
            }
            navService.Navigate(((Page)newComponent), Defaults.Integer);
        }


        /// <summary>
        /// Opens the menu based on mouse event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">EventArgs of event</param>
        public void MenuOpen(object sender, MouseEventArgs e)
        {
            var senderStrong = sender.CastOrFill<Canvas>();
            var animation = new DoubleAnimation() { From = senderStrong.Width, To = WidthOpen, Duration = TimeSpan.FromSeconds(this.AnimationDuration), AutoReverse = false, RepeatBehavior = new RepeatBehavior(1) };
            senderStrong.BeginAnimation(Canvas.WidthProperty, animation);
            this.isOpen = true;
        }

        /// <summary>
        /// Closes the menu based on mouse event
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">EventArgs of event</param>
        public void MenuClose(object sender, MouseEventArgs e)
        {
            var senderStrong = sender.CastOrFill<Canvas>();
            var animation = new DoubleAnimation()
            {
                From = senderStrong.Width,
                To = WidthClosed,
                Duration = TimeSpan.FromSeconds(this.AnimationDuration),
                AutoReverse = false,
                RepeatBehavior = new RepeatBehavior(1)
            };
            senderStrong.BeginAnimation(Canvas.WidthProperty, animation);
            this.isOpen = false;
        }

        /// <summary>
        /// Validate this control
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            return true;
        }
    }
}
