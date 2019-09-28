using System;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Exposing Microsoft Internal frame interface to force consistency cross-platform
    /// </summary>
    public interface IFrame
    {
        /// <summary>
        /// CanGoBack
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// CanGoForward
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// GoBack
        /// </summary>
        void GoBack();

        /// <summary>
        /// GoForward
        /// </summary>
        void GoForward();
    }
}
