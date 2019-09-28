using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface ICreateOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity);
    }
}
