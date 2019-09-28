using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface IDeleteOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        TEntity Delete(TEntity entity);

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <param name="entity">Entity to be deleted in the datastore</param>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        bool CanDelete(TEntity entity);
    }
}
