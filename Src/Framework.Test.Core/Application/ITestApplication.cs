using GoodToCode.Framework.Application;
using System;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface ITestApplication : IApplication, INavigateUri
    {
        /// <summary>
        /// Currently active page Uri
        /// </summary>
        Uri CurrentPage { get; }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        Uri StartupUri { get; }

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        Uri HomePage { get; }

        /// <summary>
        /// Error screen
        /// </summary>
        Uri ErrorPage { get; }
    }
}
