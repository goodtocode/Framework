using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Operation
{
    /// <summary>
    /// CRUD operations
    /// Create, Read, Update, Delete.
    ///  Purpose is to encapsulate IQueryOperation and ISaveOperationAsync for syncronous datastore access
    /// </summary>
    /// <typeparam name="TEntity">Type of class supporting CRUD methods</typeparam>
    public interface ICrudOperation<TEntity> : ICreateOperation<TEntity>, IReadOperation<TEntity>, IUpdateOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : IEntity
    {
    }
}
