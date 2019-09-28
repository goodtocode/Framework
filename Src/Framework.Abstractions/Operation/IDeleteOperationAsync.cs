using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface IDeleteOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        Task<TEntity> DeleteAsync(TEntity entity);
    }
}