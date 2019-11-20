using GoodToCode.Extensions;
using GoodToCode.Extensions.Collections;
using GoodToCode.Framework.Data;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public class EntityConfiguration<TEntity> : IEntityConfiguration<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        /// <summary>
        /// Entity data this configuration may need for stored procedure in-lining
        /// </summary>
        public TEntity EntityData { get; set; } = new TEntity();

        /// <summary>
        /// Connection String Name (key) to be used for this object's data access
        /// </summary>
        public string ConnectionName { get; set; } = ConnectionStringName.DefaultConnectionName;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Table Name to be used for this object's data access
        /// </summary>
        public string TableName { get; set; } = typeof(TEntity).Name;

        /// <summary>
        /// Table Column Prefix to be used for this object's data access
        /// </summary>
        public string ColumnPrefix { get; set; } = Defaults.String;

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
        public int RowsAffected { get; set; } = Defaults.Integer;

        /// <summary>
        /// OnModelCreating types to ignore
        /// Begins initialized with common framework utility classes
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(EntityValidator<TEntity>),
                typeof(ValidationRule<TEntity>),
                typeof(Serializer<TEntity>),
                typeof(StoredProcedure<TEntity>),
                typeof(KeyValueListString)};

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TEntity, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TEntity, object>>>()
            {
                p => p.State,
                p => p.FailedRules
            };

        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get { return new ConfigurationManagerCore(ApplicationTypes.Native).ConnectionString(ConnectionName).ToADO(); } }

        /// <summary>
        /// Stored procedure that creates the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> CreateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that updates the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> UpdateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that deletes the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> DeleteStoredProcedure { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration() : base()
        {
            var objectWithAttributes = new TEntity();
            ConnectionName = objectWithAttributes.GetAttributeValue<ConnectionStringName>(ConnectionName);
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
        public EntityConfiguration(TEntity entity) : this()
        {
            EntityData = entity;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration(string databaseSchemaName) : this()
        {
            DatabaseSchema = databaseSchemaName;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration(Expression<Func<TEntity, object>> ignoredProperty) : this()
        {
            IgnoredProperties.Add(ignoredProperty);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityConfiguration(IList<Expression<Func<TEntity, object>>> ignoredProperties) : this()
        {
            IgnoredProperties.AddRange(ignoredProperties);
        }

        /// <summary>
        /// Configures the mapping
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Table
            builder.ToTable(TableName, DatabaseSchema);
            builder.HasKey(p => p.Key);
            // Columns
            builder.Property(x => x.Id)
                .HasColumnName($"{ColumnPrefix}Id");
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
            var itemType = new TEntity();

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
            var itemType = new TEntity();

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
