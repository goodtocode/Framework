using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Write operation to a non-thread-safe datastore such as EF data context
    /// Includes all Save() and Delete() overloads, as well as Get..() methods
    /// </summary>
    public interface ISaveOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Inserts or Updates this object in the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object updated and all current values as of the save</returns>
        TEntity Save(TEntity entity);

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        bool CanInsert(TEntity entity);

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <param name="entity">Entity to be updated in the datastore</param>
        /// <returns>True if rules and setup allow for update, else false</returns>
        bool CanUpdate(TEntity entity);
    }
}
