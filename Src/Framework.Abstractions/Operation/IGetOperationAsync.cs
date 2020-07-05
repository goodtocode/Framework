using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Read operations against an Async datastore, such as Http resource server
    /// Both Id and Key can be used as 1-1 unique idenfiers
    ///  For Internal, high-performance, multi-join lookups: int Id
    ///  For External, low-volume tables, obfuscated, guaranteed-unique: Guid Key
    /// </summary>
    /// <typeparam name="TEntity">Entity type to be read</typeparam>
    public interface IGetOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Gets all items from the datastore
        /// Expects additional constraints to be attached by the caller
        ///  Usage: customer.GetAll().Where(x => x.FirstName == "Jo")
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != -1 And Also Key != Guid.Empty
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllExcludeDefaultAsync();

        /// <summary>
        /// Gets one or no items based on exact Id match
        /// </summary>
        /// <returns>One or no TEntity based on exact Id match</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Gets one or no items based on exact Key match
        /// </summary>
        /// <returns>One or no TEntity based on exact Key match</returns>
        Task<TEntity> GetByKeyAsync(Guid key);
    }
}
