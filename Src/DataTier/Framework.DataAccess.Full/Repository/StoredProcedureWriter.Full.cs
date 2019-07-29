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
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class StoredProcedureWriter<TEntity> : DbContext, 
        ICreateOperation<TEntity>, IUpdateOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : StoredProcedureEntity<TEntity>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StoredProcedureWriter()
            : base(ConnectionStringGet())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<StoredProcedureWriter<TEntity>>(null);
        }

        /// <summary>
        /// Constructor. Explicitly set database connection.
        /// </summary>
        /// <param name="connectionString">Connection String to be used for this object data access</param>
        protected StoredProcedureWriter(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<StoredProcedureWriter<TEntity>>(null);
        }

        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object pulled from datastore</returns>
        public TEntity Create(TEntity entity)
        {
            try
            {
                if (entity.CreateStoredProcedure == null) throw new Exception("Create() requires CreateStoredProcedure to be initialized properly.");
                if (entity.IsValid() && CanInsert(entity))
                {
                    entity.Key = entity.Key == Defaults.Guid ? Guid.NewGuid() : entity.Key; // Required to re-pull data after save
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.CreateStoredProcedure);
                    if (rowsAffected > 0) entity.Fill(Read(x => x.Key == entity.Key).FirstOrDefaultSafe());  // Re-pull clean object, exactly as the DB has stored
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Create() on {this.ToString()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Retrieve TEntity objects operation
        /// Default: Does Not read from a Get stored procedure.
        ///  Reads directly from DbSet defined in repository class. 
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns>IQueryable of read operation</returns>
        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression)
        {
            return Data.Where(expression);
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public TEntity Update(TEntity entity)
        {
            try
            {
                if (entity.UpdateStoredProcedure == null) throw new Exception("Update() requires UpdateStoredProcedure to be initialized properly.");
                if (entity.IsValid() && CanUpdate(entity))
                {
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.UpdateStoredProcedure);
                    if (rowsAffected > 0) entity.Fill(Read(x => x.Key == entity.Key).FirstOrDefaultSafe());
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Update() on {this.ToString()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public virtual TEntity Save(TEntity entity)
        {
            if (CanInsert(entity))
                entity = Create(entity);
            else if (CanUpdate(entity))
                entity = Update(entity);
            return entity;
        }

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        public TEntity Delete(TEntity entity)
        {
            try
            {
                if (entity.DeleteStoredProcedure == null) throw new Exception("Delete() requires DeleteStoredProcedure to be initialized properly.");
                if (CanDelete(entity))
                {
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.DeleteStoredProcedure);
                    if (rowsAffected > 0) entity.Fill(Read(x => x.Key == entity.Key).FirstOrDefaultSafe());
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Delete() on {this.ToString()}");
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
            var configDb = new EntityConfiguration<TEntity>();
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
            if ((DatabaseConfig.ConnectionString.Length == 0 || !CanConnect))
                throw new Exception("Database connection failed or the connection string could not be found. A valid connection string required for data access.");
            modelBuilder.HasDefaultSchema(DatabaseConfig.DatabaseSchema);
            // Table
            modelBuilder.Entity<TEntity>().ToTable(DatabaseConfig.TableName);
            modelBuilder.Entity<TEntity>().HasKey(p => p.Key);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            // Columns
            modelBuilder.Entity<TEntity>().Property(x => x.Id)
                .HasColumnName($"{DatabaseConfig.ColumnPrefix}Id");
            modelBuilder.Entity<TEntity>().Property(x => x.Key)
                .HasColumnName($"{DatabaseConfig.ColumnPrefix}Key");
            // Ignored            
            modelBuilder.Ignore(DatabaseConfig.IgnoredTypes);
            foreach (var item in DatabaseConfig.IgnoredProperties)
            {
                modelBuilder.Entity<TEntity>().Ignore(item);
            }

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Executes stored procedure for specific parameter behavior
        /// Named: @Param1 is used to match parameter with entity data. 
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
        /// Ordinal: @Param1, @Param2 are assigned in ordinal position
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
        /// </summary>
        public int ExecuteSqlCommand(StoredProcedure<TEntity> storedProc)
        {
            var returnValue = Defaults.Integer;
            switch (ParameterBehavior)
            {
                case ParameterBehaviors.Named:
                    returnValue = Database.ExecuteSqlCommand(storedProc.ToString());
                    break;
                case ParameterBehaviors.Ordinal:
                default:
                    returnValue = Database.ExecuteSqlCommand(storedProc.SqlPrefix, storedProc.Parameters.ToArray());
                    break;
            }
            return returnValue;
        }
    }
}
