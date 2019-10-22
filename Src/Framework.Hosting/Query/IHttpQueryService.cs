using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// HttpSearchService contract
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IHttpQueryService<TDto> : IHttpService
    {
        /// <summary>
        /// Name of the type that is mapped to the query
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Querystring parameters, well formed
        /// </summary>
        Uri FullUri { get; }

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="query">Querystring parameters that will result in one item returned</param>
        /// <returns>Item from the system</returns>
        Task<List<TDto>> QueryAsync(string query);
    }
}