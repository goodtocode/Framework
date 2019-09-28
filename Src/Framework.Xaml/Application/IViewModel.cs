using GoodToCode.Extensions.Net;
using System.ComponentModel;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Interface to enforce ViewModel division of responsibilities
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IViewModel<TModel> : INotifyPropertyChanged
    {
        /// <summary>
        /// Configuration data
        ///  Data must be constructed in the application tier
        /// </summary>
        IApplication MyApplication { get; }

        /// <summary>
        /// Model data
        /// </summary>
        TModel MyModel { get; }

        /// <summary>
        /// Sender of main Http Verbs
        /// </summary>
        HttpVerbSender Sender { get; }
    }
}
