using System;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateType
    {
        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Type destinationPageType);

        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate<TModel>(Type destinationPageType, TModel dataToPass);
    }

    /// <summary>
    /// Application frame/page navigation
    /// </summary>
    public interface INavigateType<TModel> : INavigateType
    {

        /// <summary>
        /// Navigates to the specified type, typically a Wpf or Universal page
        /// </summary>
        /// <param name="destinationPageType">Type to navigate, typically a Wpf or Universal page</param>
        /// <param name="dataToPass">Object to pass to the destination</param>
        /// <returns>False if navigation fails</returns>
        bool Navigate(Type destinationPageType, TModel dataToPass);
    }
}
