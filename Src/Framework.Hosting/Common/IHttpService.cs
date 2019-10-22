using System;
using System.Net.Http;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// IHttpService contract
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Uri of the Query RESTful endpoint
        /// </summary>
        Uri Uri { get; set; }

        /// <summary>
        /// Response from the request
        /// </summary>
        HttpResponseMessage Response { get; set; }
    }
}