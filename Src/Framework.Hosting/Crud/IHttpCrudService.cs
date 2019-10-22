using System;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// HttpCrudService contract
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    public interface IHttpCrudService<TDto> : IHttpService
    {
        /// <summary>
        /// Name of the type that is mapped to the query
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Creates an item in the system
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Created item, with updated Id/Key (if applicable)</returns>
        Task<TDto> CreateAsync(TDto item);

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="query">Querystring parameters that will result in one item returned</param>
        /// <returns>Item from the system</returns>
        Task<TDto> ReadAsync(string query);

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="id">Id of the item to return</param>
        /// <returns>Item from the system</returns>
        Task<TDto> ReadAsync(int id);

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="key">Key of the item to return</param>
        /// <returns>Item from the system</returns>
        Task<TDto> ReadAsync(Guid key);

        /// <summary>
        /// Updates an item in the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="item">item to update</param>
        /// <returns>Item from the system</returns>
        Task<TDto> UpdateAsync(TDto item);

        /// <summary>
        /// Deletes an item in the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="item">item to delete</param>
        /// <returns>Item from the system</returns>
        Task<bool> DeleteAsync(TDto item);
    }
}