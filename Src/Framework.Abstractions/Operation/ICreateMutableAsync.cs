using GoodToCode.Framework.Data;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveMutableAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface ICreateMutableAsync<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns></returns>
        Task<TEntity> CreateAsync(TEntity entity);

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        bool CanCreate(TEntity entity);
    }
}