using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Entity
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
        DbSet<TEntity> Data { get; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        IEntityWriterConfiguration<TEntity> ConfigOptions { get; }

        /// <summary>
        /// Can connect to database?
        /// </summary>
        bool CanConnect { get; }
    }
}
