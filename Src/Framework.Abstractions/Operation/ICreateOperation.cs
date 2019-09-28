using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface ICreateOperation<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object pulled from datastore</returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        bool CanInsert(TEntity entity);
    }
}
