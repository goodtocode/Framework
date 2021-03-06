using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Value
{
    /// <summary>
    /// Database connection and metadata info
    /// </summary>
    public interface IValueReaderConfiguration<TValue> : IEntityTypeConfiguration<TValue> where TValue : ValueBase<TValue>, new()
    {
        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        string DatabaseSchema { get; set; }

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
        /// List of types to ignore in database operations
        /// </summary>
        IList<Type> IgnoredTypes { get; set; }

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
    }
}
