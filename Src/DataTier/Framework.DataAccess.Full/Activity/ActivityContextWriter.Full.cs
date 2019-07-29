//-----------------------------------------------------------------------
// <copyright file="ActivityContext.cs" company="GoodToCode">
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
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GoodToCode.Extensions;
using GoodToCode.Extras.Configuration;
using GoodToCode.Extras.Data;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Data context for ActivityContext class
    /// </summary>
    public class ActivityContextWriter : DbContext
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<ActivityContext> Data { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityContextWriter()
            : base(ConnectionStringGet())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Database.SetInitializer<ActivityContextWriter>(null);
        }

        /// <summary>
        /// Fills and saves an activity
        /// </summary>
        /// <remarks></remarks>
        public static ActivityContext Create()
        {
            var returnValue = new ActivityContext();

            returnValue = new ActivityContextWriter().Save(returnValue);

            return returnValue;
        }

        /// <summary>
        /// Saves object to database
        /// Silent errors,, Activity is an 100% uptime namespace
        /// </summary>
        public virtual ActivityContext Save(ActivityContext entity)
        {
            var returnValue = entity;

            if (entity.ActivityContextId == Defaults.Integer)
            {
                entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? Guid.NewGuid() : entity.ActivityContextKey;
                Entry(entity).State = EntityState.Added;
                var results = SaveChanges();
                returnValue = new ActivityContextReader().GetByKey(entity.ActivityContextKey);
            }

            return returnValue;
        }

        /// <summary>
        /// Gets connection strings from connectionstrings.config
        /// </summary>
        /// <returns></returns>
        private static string ConnectionStringGet()
        {
            var returnValue = Defaults.String;
            var configManager = new ConfigurationManagerFull();
            var connectionName = new ActivityContext().GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName);
            var configConnectString = configManager.ConnectionString(connectionName).ToADO();

            if (configConnectString.Length > 0)
            {
                returnValue = configConnectString;
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
            var databaseSchema = new ActivityContext().GetAttributeValue<DatabaseSchemaName>(DatabaseSchemaName.DefaultDatabaseSchema);
            modelBuilder.HasDefaultSchema(databaseSchema);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Initializes database for Code First classes
        /// </summary>
        private class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ActivityContextWriter>
        {
            /// <summary>
            /// Sets default data
            /// </summary>
            /// <param name="context">User, device and app context</param>
            protected override void Seed(ActivityContextWriter context)
            {
                base.Seed(context);
            }
        }
    }
}
