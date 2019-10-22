using System;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// Uri for service options
    /// </summary>
    public interface IUriOption
    {
        /// <summary>
        /// Url for the option
        /// </summary>
        Uri Url { get; set; }
    }

    /// <summary>
    /// Uri for service options
    /// </summary>
    public class UriOption : IUriOption
    {
        /// <summary>
        /// Url for the option
        /// </summary>
        public Uri Url { get; set; }
    }
}
