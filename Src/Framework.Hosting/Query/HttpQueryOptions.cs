
using System.Collections.Generic;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// HttpSearchService contract
    /// </summary>
    public class HttpQueryOptions : List<HttpQueryOption> { }

    /// <summary>
    /// HttpSearchService contract
    /// </summary>
    public class HttpQueryOption
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