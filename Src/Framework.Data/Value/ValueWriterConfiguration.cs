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
    public class ValueWriterConfiguration<TValue> : IValueWriterConfiguration<TValue> where TValue : ValueBase<TValue>, new()
    {
        /// <summary>
        /// Entity data this configuration may need for stored procedure in-lining
        /// </summary>
        public TValue Entity { get; set; } = new TValue();

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
        /// Concurrency setting for contention management
        /// </summary>
        public DataConcurrencies DataConcurrency { get; set; } = DataConcurrencies.Optimistic;

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        public DataAccessBehaviors DataAccessBehavior { get; set; } = DataAccessBehaviors.AllAccess;

        /// <summary>
        /// Defines how stored prorcedure parameters are invoked
        /// </summary>
        public ParameterBehaviors ParameterBehavior { get; set; } = ParameterBehaviors.Named;

        /// <summary>
        /// Number of rows affected by any operation
        /// </summary>
        public int RowsAffected { get; set; } = -1;

        /// <summary>
        /// OnModelCreating types to ignore
        /// Begins initialized with common framework utility classes
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(ValidationRule<TValue>),
                typeof(Serializer<TValue>),
                typeof(StoredProcedure<TValue>),
                typeof(List<KeyValuePair<string, string>>)};

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TValue, object>>>()
            {
                p => p.State
            };

        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get; } = string.Empty;

        /// <summary>
        /// Stored procedure that creates the entity
        /// </summary>
        public virtual StoredProcedure<TValue> CreateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that updates the entity
        /// </summary>
        public virtual StoredProcedure<TValue> UpdateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that deletes the entity
        /// </summary>
        public virtual StoredProcedure<TValue> DeleteStoredProcedure { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public ValueWriterConfiguration(string connectionString) : base()
        {
            var objectWithAttributes = new TValue();
            ConnectionString = connectionString;
            DatabaseSchema = objectWithAttributes.GetAttributeValue<DatabaseSchemaName>(DatabaseSchema);
            TableName = objectWithAttributes.GetAttributeValue<TableName>(TableName);
            ColumnPrefix = objectWithAttributes.GetAttributeValue<ColumnPrefix>(ColumnPrefix);
            DataConcurrency = DataConcurrencyGet(DataConcurrencies.Optimistic);
            DataAccessBehavior = DataAccessBehaviorGet();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="connectionString"></param>
        public ValueWriterConfiguration(string connectionString, TValue entity) : this(connectionString)
        {
            Entity = entity;            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseSchemaName"></param>
        public ValueWriterConfiguration(string connectionString, string databaseSchemaName) : this(connectionString)
        {
            DatabaseSchema = databaseSchemaName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredProperty"></param>
        public ValueWriterConfiguration(string connectionString, Expression<Func<TValue, object>> ignoredProperty) : this(connectionString)
        {
            IgnoredProperties.Add(ignoredProperty);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredProperties"></param>
        public ValueWriterConfiguration(string connectionString, IList<Expression<Func<TValue, object>>> ignoredProperties) : this(connectionString)
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

        /// <summary>
        /// Concurrency model to follow in middle tier, and optionally in the data tier
        /// </summary>
        /// <returns></returns>
        private DataConcurrencies DataConcurrencyGet(DataConcurrencies defaultValue)
        {
            var returnValue = defaultValue;
            var itemType = new TValue();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataConcurrency))
                {
                    returnValue = ((DataConcurrency)item).Value;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Defines if object can select, insert, update and/or delete.
        /// </summary>
        internal DataAccessBehaviors DataAccessBehaviorGet()
        {
            var returnValue = DataAccessBehaviors.AllAccess;
            var itemType = new TValue();

            foreach (var item in itemType.GetType().GetCustomAttributes(false))
            {
                if ((item is DataAccessBehavior))
                {
                    returnValue = ((DataAccessBehavior)item).Value;
                    break;
                }
            }

            return returnValue;
        }
    }
}