using GoodToCode.Framework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// Database connection and metadata info
    /// </summary>
    public interface IEntityReaderConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase<TEntity>, new()
    {        
        /// <summary>
        /// Connection String (full string) to be used for this object's data access
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// List of types to ignore in database operations
        /// </summary>
        IList<Type> IgnoredTypes { get; set; }

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        IList<Expression<Func<TEntity, object>>> IgnoredProperties { get; set; }
    }
}
