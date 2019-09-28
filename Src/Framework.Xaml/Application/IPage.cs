using GoodToCode.Extensions.Net;
using System;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Screen base for authenticated users
    /// </summary>    
    interface IPage
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        IApplication MyApplication { get; }

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        HttpVerbSender HttpSender { get; set; }

        /// <summary>
        /// Name of the controller used in web service calls
        /// </summary>
        string ControllerName { get; }

        /// <summary>
        /// Uri to currently active frame/page
        /// </summary>
        Uri CurrentPage { get; }

        /// <summary>
        /// Throws Exception if any UI elements overrun their text max length
        /// </summary>
        bool ThrowExceptionOnTextOverrun { get; set; }
                
        /// <summary>
        /// Binds all model data to the screen controls and sets MyViewModel.MyModel property
        /// </summary>
        /// <param name="modelData">Model data to bind</param>
        void BindModel(object modelData);

        /// <summary>
        /// Page_Load event handler
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        void Page_Loaded(object sender, EventArgs e);      
    }
}
