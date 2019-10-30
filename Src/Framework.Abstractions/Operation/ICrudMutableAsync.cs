using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveMutableAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface ICrudMutableAsync<TEntity> : ICreateMutableAsync<TEntity>, IReadMutableAsync<TEntity>, IUpdateMutableAsync<TEntity>, IDeleteMutableAsync<TEntity> where TEntity : IEntity
    {        
    }
}
