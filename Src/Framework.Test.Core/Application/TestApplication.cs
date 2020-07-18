
using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Net;
using System;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Global application information
    /// </summary>
    public class TestApplication : ITestApplication
    {

        /// <summary>
        /// MyWebService
        /// </summary>
        public Uri MyWebService { get { return new Uri(new AppSettingFactory().MyWebService, UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public Uri StartupUri { get; } = new Uri("http://localhost:80", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public Uri HomePage { get; } = new Uri("http://localhost:80", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Error screen
        /// </summary>
        public Uri ErrorPage { get; } = new Uri("http://localhost:80", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Constructor
        /// </summary>
        public TestApplication() : base()
        {
            OnObjectInitialize();
        }

        /// <summary>
        /// Loads config data
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            await Task.Delay(1);            
        }

        /// <summary>
        /// Wakes up any sleeping processes, and MyWebService chain
        /// </summary>
        /// <returns></returns>
        public virtual async Task WakeServicesAsync()
        {
            if (MyWebService.ToString() == string.Empty)
            {
                HttpRequestGetString Request = new HttpRequestGetString(MyWebService.ToString())
                {
                    ThrowExceptionWithEmptyReponse = false
                };
                await Request.SendAsync();
            }
        }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get; } = false;

        /// <summary>
        /// Can this screen go forward
        /// </summary>
        public bool CanGoForward { get; } = false;

        /// <summary>
        /// Current loaded page
        /// </summary>
        public Uri CurrentPage { get; } = new Uri("http://localhost:80", UriKind.RelativeOrAbsolute);

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { }

        /// <summary>
        /// Navigates forward to next screen
        /// </summary>
        public void GoForward() { }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        public bool Navigate(Uri destinationPageUrl) { return true; }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<TModel>(Uri destinationPageUrl, TModel dataToPass) { return true; }
        
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
