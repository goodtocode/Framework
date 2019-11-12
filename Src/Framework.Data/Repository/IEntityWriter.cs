using GoodToCode.Framework.Entity;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public interface IEntityWriter<TEntity> : ISaveOperationAsync<TEntity>, ICreateOperationAsync<TEntity>, IUpdateOperationAsync<TEntity>, IDeleteOperationAsync<TEntity> 
        where TEntity : EntityBase<TEntity>, new()
    {
        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>
        TEntity Entity { get; }

        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        IEntityConfiguration<TEntity> ConfigOptions { get; set; }

        /// <summary>
        /// Can connect to database?
        /// </summary>
        bool CanConnect { get; }
    }
}
