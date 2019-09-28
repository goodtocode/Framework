using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface IReadOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Retrieve TEntity objects operations
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        Task<IList<TEntity>> ReadAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Retrieve TEntity objects operations
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        Task<TEntity> ReadSingleAsync(Expression<Func<TEntity, bool>> expression);
    }
}
