using GoodToCode.Framework.Data;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface IReadOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Retrieve TEntity objects operation
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns></returns>
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression);        
    }
}
