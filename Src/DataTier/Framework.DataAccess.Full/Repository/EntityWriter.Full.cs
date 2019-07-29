//-----------------------------------------------------------------------
// <copyright file="EntityWriter.cs" company="GoodToCode">
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
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class EntityWriter<TEntity> : DbContext, ISaveOperation<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityWriter()
            : base(ConnectionStringGet())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<EntityWriter<TEntity>>(null);
        }

        /// <summary>
        /// Constructor. Explicitly set database connection.
        /// </summary>
        /// <param name="connectionString">Connection String to be used for this object data access</param>
        protected EntityWriter(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<EntityWriter<TEntity>>(null);
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public virtual TEntity Save(TEntity entity)
        {
            var activity = new ActivityContext();
            var trackingState = EntityState.Unchanged;

            try
            {
                entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                if (CanInsert(entity))
                {
                    trackingState = EntityState.Added;
                    Data.Add(entity);
                }
                else if (CanUpdate(entity))
                    trackingState = EntityState.Modified;
                if (entity.IsValid() && trackingState != EntityState.Unchanged)
                {
                    entity.Key = entity.Key == Defaults.Guid ? Guid.NewGuid() : entity.Key; // Required to re-pull data after save
                    Entry(entity).State = trackingState;
                    SaveChanges();
                    entity.Fill(new EntityReader<TEntity>().GetByKey(entity.Key)); // Re-pull clean object, exactly as the DB has stored
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Save() on {this.ToString()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Worker that deletes this object with automatic tracking
        /// </summary>      
        /// <returns>True if record deleted, false if not</returns>
        public virtual TEntity Delete(TEntity entity)
        {
            try
            {
                if (CanDelete(entity))
                {
                    Entry(entity).State = EntityState.Deleted;
                    Data.Remove(entity);
                    SaveChanges();
                    entity = new EntityReader<TEntity>().GetByKey(entity.Key); // Re-pull clean object, exactly as the DB has stored
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Delete() on {this.ToString()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Gets connection strings from connectionstrings.config
        /// </summary>
        /// <returns></returns>
        private static string ConnectionStringGet()
        {
            var returnValue = Defaults.String;
            var dbAdapter = new EntityConfiguration<TEntity>();
            var configManager = new ConfigurationManagerFull();
            var configConnectString = configManager.ConnectionString(dbAdapter.ConnectionName);
            var adoConnectionString = configConnectString.ToADO();

            if (adoConnectionString.Length > 0)
                returnValue = adoConnectionString;
            else
                throw new Exception("Connection string could not be found. A valid connection string required for data access.");

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
            modelBuilder.Entity<TEntity>().ToTable(ConfigOptions.TableName);
            modelBuilder.Entity<TEntity>().HasKey(p => p.Key);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Columns
            modelBuilder.Entity<TEntity>().Property(x => x.Key)
                .HasColumnName($"{ConfigOptions.ColumnPrefix}Key");
            // Ignored            
            modelBuilder.Ignore(ConfigOptions.IgnoredTypes);
            foreach (var item in ConfigOptions.IgnoredProperties)
            {
                modelBuilder.Entity<TEntity>().Ignore(item);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Initializes database for Code First classes
        /// </summary>
        private class DatabaseInitializer : DropCreateDatabaseIfModelChanges<EntityWriter<TEntity>>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context">User, device and app context</param>
            protected override void Seed(EntityWriter<TEntity> context)
            {
                base.Seed(context);
            }
        }
    }
}
