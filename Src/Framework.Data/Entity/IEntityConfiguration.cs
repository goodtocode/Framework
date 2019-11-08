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
    public partial interface IEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        string DatabaseSchema { get; set; }

        /// <summary>
        /// Connection String Name (key only) to be used for this object's data access
        /// </summary>
        string ConnectionName { get; set; }

        /// <summary>
        /// Connection String (full string) to be used for this object's data access
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Table this configuration will interact
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Prefix for all table columns. 
        ///  Default/No prefix: Property names must  match the column like Id, Key, FirstName 
        ///  Prefix provided is applied to all: CustomerId, CustomerKey, CustomerFirstName
        /// </summary>
        string ColumnPrefix { get; set; }

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        DataConcurrencies DataConcurrency { get; set; }

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        DataAccessBehaviors DataAccessBehavior { get; set; }

        /// <summary>
        /// Rows affected by any adapter operation
        /// </summary>
        int RowsAffected { get; }

        /// <summary>
        /// List of types to ignore in database operations
        /// </summary>
        IList<Type> IgnoredTypes { get; }

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        IList<Expression<Func<TEntity, object>>> IgnoredProperties { get; set; }
    }
}
