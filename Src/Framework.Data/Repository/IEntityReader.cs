using GoodToCode.Framework.Entity;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoodToCode.Framework.Repository
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
        IEntityConfiguration<TEntity> ConfigOptions { get; set; }

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
