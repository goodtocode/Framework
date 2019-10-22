
using System.Collections.Generic;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// HttpSearchService contract
    /// </summary>
    public class HttpCrudOptions : List<HttpCrudOption> {    }

    /// <summary>
    /// HttpSearchService contract
    /// </summary>
    public class HttpCrudOption
    {
        /// <summary>
        /// Url of the query
        /// </summary>
        public string Type { get; set; } = default(string);

        /// <summary>
        /// Url of the query
        /// </summary>
        public string Url { get; set; } = default(string);
    }
}