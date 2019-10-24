using GoodToCode.Extensions.Configuration;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// EF to SQL View for this object
    /// </summary>
    public class ValueConfiguration<TValue> : IValueConfiguration<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// EF DbContext class so adapter can perform the read/write
        /// </summary>
        public DbContext Context { get; set; }

        /// <summary>
        /// Connection string as read from the config file, or passed as a constructor parameter
        /// </summary>
        public string ConnectionString { get { return new ConfigurationManagerCore(ApplicationTypes.Native).ConnectionString(ConnectionName).ToADO(); } }

        /// <summary>
        /// Configures the mapping
        /// </summary>
        /// <param name="builder"></param>
        public virtual void Configure(EntityTypeBuilder<TValue> builder)
        {
            // Table
            builder.ToTable(TableName, DatabaseSchema);
            builder.HasKey(p => p.Key);
            // Columns
            builder.Property(x => x.Key)
                .HasColumnName("Key");
            // Ignored
            foreach (var property in IgnoredProperties)
            {
                builder.Ignore(property);
            }
            builder.Ignore("ThrowException");
        }

        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns></returns>
        public TValue Create(TValue entity)
        {
            Context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public TValue Update(TValue entity)
        {
            Context.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        public TValue Delete(TValue entity)
        {
            Context.SaveChanges();
            return entity;
        }
    }
}
