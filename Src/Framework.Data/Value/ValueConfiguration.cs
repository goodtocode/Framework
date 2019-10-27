using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Extensions.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Value
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public class ValueConfiguration<TValue> : IValueConfiguration<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get { return new ConfigurationManagerCore(ApplicationTypes.Native).ConnectionString(ConnectionName).ToADO(); } }

        /// <summary>
        /// Connection String Name (key) to be used for this object's data access
        /// </summary>
        public string ConnectionName { get; set; } = ConnectionStringName.DefaultConnectionName;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string TableName { get; set; } = typeof(TValue).Name;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string ColumnPrefix { get; set; } = string.Empty;

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public DataConcurrencies DataConcurrency { get; set; } = DataConcurrencies.Optimistic;

        /// <summary>
        /// Data access behavior of this instance.
        /// </summary>
        public DataAccessBehaviors DataAccessBehavior { get; } = DataAccessBehaviors.SelectOnly;

        /// <summary>
        /// OnModelCreating types to ignore
        /// </summary>
        public IList<Type> IgnoredTypes { get; set; }
            = new List<Type>() {
                typeof(Serializer<TValue>) };

        /// <summary>
        /// Properties to ignore in the database mapping
        /// </summary>
        public IList<Expression<Func<TValue, object>>> IgnoredProperties { get; set; }
            = new List<Expression<Func<TValue, object>>>() {
                p => p.State };

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueConfiguration() : base()
        {
            var objectWithAttributes = new TValue();
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
        public ValueConfiguration(string databaseSchemaName) : this()
        {
            DatabaseSchema = databaseSchemaName;
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
