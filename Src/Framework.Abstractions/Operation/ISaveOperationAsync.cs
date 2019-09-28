using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface ISaveOperationAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object updated and all current values as of the save</returns>
        Task<TEntity> SaveAsync(TEntity entity);
    }
}
