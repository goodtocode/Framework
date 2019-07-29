//-----------------------------------------------------------------------
// <copyright file="ValueReader.cs" company="GoodToCode">
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
using GoodToCode.Extensions;
using GoodToCode.Extras.Configuration;
using GoodToCode.Framework.Data;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class ValueReader<TValue> : DbContext where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the Value.
        /// </summary>
        public DbSet<TValue> Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueReader()
            : base(ConnectionStringGet())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<ValueReader<TValue>>(null);
        }

        /// <summary>
        /// Constructor. Explicitly set database connection.
        /// </summary>
        /// <param name="connectionString">Connection String to be used for this object data access</param>
        protected ValueReader(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<ValueReader<TValue>>(null);
        }

        /// <summary>
        /// Gets connection strings from connectionstrings.config
        /// </summary>
        /// <returns></returns>
        private static string ConnectionStringGet()
        {
            var returnValue = Defaults.String;
            var configDb = new ValueConfiguration<TValue>();
            var configManager = new ConfigurationManagerFull();
            var configConnectString = configManager.ConnectionString(configDb.ConnectionName);
            var adoConnectionString = configConnectString.ToADO();

            if (adoConnectionString.Length > 0)
            {
                returnValue = adoConnectionString;
            }
            else
            {
                throw new Exception("Connection string could not be found. A valid connection string required for data access.");
            }

            return returnValue;
        }

        /// <summary>
        /// Set values when creating a model in the database
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <remarks></remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if ((ConfigOptions.ConnectionString.Length == 0 || !CanConnect))
                throw new Exception("Database connection failed or the connection string could not be found. A valid connection string required for data access.");
            modelBuilder.HasDefaultSchema(ConfigOptions.DatabaseSchema);
            // Table
            modelBuilder.Entity<TValue>().ToTable(ConfigOptions.TableName);
            modelBuilder.Entity<TValue>().HasKey(p => p.Key);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Columns
            modelBuilder.Entity<TValue>().Property(x => x.Key)
                .HasColumnName($"{ConfigOptions.ColumnPrefix}Key");
            // Ignored            
            modelBuilder.Ignore(ConfigOptions.IgnoredTypes);
            foreach(var item in ConfigOptions.IgnoredProperties)
            {
                modelBuilder.Entity<TValue>().Ignore(item);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
