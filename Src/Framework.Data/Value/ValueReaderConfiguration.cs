using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Value
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public class ValueReaderConfiguration<TValue> : IValueReaderConfiguration<TValue> where TValue : ValueBase<TValue>, new()
    {
        /// <summary>
        /// OnModelCreating types to ignore
        /// Begins initialized with common framework utility classes
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(ValidationRule<TValue>),
                typeof(Serializer<TValue>),
                typeof(List<KeyValuePair<string, string>>)};

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TValue, object>>>()
            {
                p => p.State,
            };

        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get; } = string.Empty;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Table Name to be used for this object's data access
        /// </summary>
        public string TableName { get; set; } = typeof(TValue).Name;

        /// <summary>
        /// Table Column Prefix to be used for this object's data access
        /// </summary>
        public string ColumnPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ValueReaderConfiguration(string connectionString) : base()
        {
            var objectWithAttributes = new TValue();
            ConnectionString = connectionString;
            DatabaseSchema = objectWithAttributes.GetAttributeValue<DatabaseSchemaName>(DatabaseSchema);
            TableName = objectWithAttributes.GetAttributeValue<TableName>(TableName);
            ColumnPrefix = objectWithAttributes.GetAttributeValue<ColumnPrefix>(ColumnPrefix);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseSchemaName"></param>
        public ValueReaderConfiguration(string connectionString, string databaseSchemaName) : this(connectionString)
        {
            DatabaseSchema = databaseSchemaName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredProperty"></param>
        public ValueReaderConfiguration(string connectionString, Expression<Func<TValue, object>> ignoredProperty) : this(connectionString)
        {
            IgnoredProperties.Add(ignoredProperty);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredProperties"></param>
        public ValueReaderConfiguration(string connectionString, IList<Expression<Func<TValue, object>>> ignoredProperties) : this(connectionString)
        {
            foreach (var item in ignoredProperties)
                IgnoredProperties.Add(item);
        }

        /// <summary>
        /// Configures the mapping
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<TValue> builder)
        {
            // Table
            builder.ToTable(TableName, DatabaseSchema);
            builder.HasKey(p => p.Key);
            // Columns
            builder.Property(x => x.Key)
                .HasColumnName($"{ColumnPrefix}Key");
            // Ignored
            foreach (var property in IgnoredProperties)
            {
                builder.Ignore(property);
            }
            builder.Ignore("ThrowException");
        }
    }
}