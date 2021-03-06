using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public interface IEntityReader<TEntity> : IGetOperation<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        IEntityReaderConfiguration<TEntity> ConfigOptions { get; }

        /// <summary>
        /// Results from any query operation
        /// </summary>
        IQueryable<TEntity> Results { get; }

        /// <summary>
        /// Can connect to database?
        /// </summary>
        bool CanConnect { get; }
    }
}
