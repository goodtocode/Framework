//-----------------------------------------------------------------------
// <copyright file="ConfigurationValue.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
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
    public partial class ValueConfiguration<TValue> : IValueConfiguration<TValue> where TValue : ValueInfo<TValue>, new()
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
