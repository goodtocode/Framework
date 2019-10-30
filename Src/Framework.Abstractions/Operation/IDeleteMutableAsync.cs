using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface IDeleteMutableAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        Task<TEntity> DeleteAsync(TEntity entity);

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        bool CanDelete(TEntity entity);
    }
}