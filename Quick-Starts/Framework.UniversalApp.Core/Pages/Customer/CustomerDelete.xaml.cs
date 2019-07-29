//-----------------------------------------------------------------------
// <copyright file="CustomerDelete.cs" company="Genesys Source">
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
using Framework.Customer;
using Framework.UserControls;
using Genesys.Extensions;
using Genesys.Framework.Worker;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Framework.Pages
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CustomerDelete : SaveablePage
    {
        /// <summary>
        /// Uri to this resource
        /// </summary>
        public static Uri Uri = new Uri("/Pages/Customer/CustomerDelete.xaml", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Controller route that handles requests for this page
        /// </summary>
        public override string ControllerName { get; } = "Customer";

        /// <summary>
        /// ViewModel holds model and is responsible for server calls, navigation, etc.
        /// </summary>
        public UniversalViewModel<CustomerModel> MyViewModel { get; }

        /// <summary>
        /// Page and controls have been loaded
        /// </summary>
        /// <param name="sender">Sender of this event call</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_Loaded(object sender, RoutedEventArgs e)
        {
            base.Page_Loaded(sender, e);
        }

        /// <summary>
        /// Sets casing
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        private void TextAll_LostFocus(object sender, RoutedEventArgs e)
        {
            TextFirstName.Text = this.TextFirstName.Text.ToPascalCase();
            TextLastName.Text = this.TextLastName.Text.ToPascalCase();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerDelete()
        {
            InitializeComponent();
            TextFirstName.LostFocus += TextAll_LostFocus;
            TextLastName.LostFocus += TextAll_LostFocus;
            MyViewModel = new UniversalViewModel<CustomerModel>(ControllerName);
        }

        /// <summary>
        /// Sets model data, binds to controls and handles event that introduce new model data to page
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        protected override void Page_ModelReceived(object sender, NewModelReceivedEventArgs e)
        {
            OkCancel.StartProcessing("Loading data...");
            var model = e.NewModelData.CastOrFill<CustomerModel>();
            BindModel(model);
            OkCancel.CancelProcessing();
        }

        /// <summary>
        /// Binds new model data to screen
        /// </summary>
        /// <param name="modelData"></param>
        protected override void BindModel(object modelData)
        {
            MyViewModel.MyModel = modelData.CastOrFill<CustomerModel>();
            DataContext = MyViewModel.MyModel;
            SetBinding(ref this.TextId, MyViewModel.MyModel.Id.ToString(), "Id");
            SetBinding(ref this.TextKey, MyViewModel.MyModel.Key.ToString(), "Key");
            SetBinding(ref this.TextFirstName, MyViewModel.MyModel.FirstName, "FirstName");
            SetBinding(ref this.TextLastName, MyViewModel.MyModel.LastName, "LastName");
            SetBinding(ref this.TextBirthDate, MyViewModel.MyModel.BirthDate.ToString(), "BirthDate");
            TextGender.Text = GenderList.GetAll().Find(x => x.Key == MyViewModel.MyModel.GenderId).Value;
        }

        /// <summary>
        /// Link actual XAML buttons to base class
        ///  A XAML template will eliminate need for this.
        /// </summary>
        protected override OkCancel OkCancel
        {
            get { return ButtonOkCancel; }
            set { ButtonOkCancel = value; }
        }

        /// <summary>
        /// Processes any page data via workflow
        /// </summary>
        public override async Task<WorkerResult> Process()
        {
            var returnValue = new WorkerResult();

            MyViewModel.MyModel = await MyViewModel.DeleteAsync(MyViewModel.MyModel);
            BindModel(MyViewModel.MyModel);
            if (!MyViewModel.MyModel.IsNew)
            {
                returnValue.FailedRules.Add("1026", "Failed to delete");
            }

            return returnValue;
        }

        /// <summary>
        /// Cancels the  and/or process
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public override void Cancel()
        {
            MyViewModel.GoBack();
        }
    }
}
