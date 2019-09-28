using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// Update operations
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface IUpdateOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Update operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object pulled from datastore</returns>
        TEntity Update(TEntity entity);

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <param name="entity">Entity to be updated in the datastore</param>
        /// <returns>True if rules and setup allow for update, else false</returns>
        bool CanUpdate(TEntity entity);
    }
}
