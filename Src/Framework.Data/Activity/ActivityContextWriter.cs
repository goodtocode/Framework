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
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

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
        /// Sets |DataDirectory| value for local SQL files (mdf)
        /// </summary>
        public ActivityContextWriter() : base()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public ActivityContextWriter(DbContextOptions<ActivityContextWriter> options) : base(options) { }

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

            try
            {
                if (entity.ActivityContextId == Defaults.Integer)
                {
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? Guid.NewGuid() : entity.ActivityContextKey;
                    Entry(entity).State = EntityState.Added;
                    SaveChanges();
                    returnValue = new ActivityContextReader().GetByKey(entity.ActivityContextKey);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(ActivityContextWriter), $"ActivityContextWriter.Save() on {this.ToString()}");
                throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Set values when creating a model in the database
        /// </summary>
        /// <param name="options"></param>
        /// <remarks></remarks>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                var connectionString = new ConfigurationManagerCore(ApplicationTypes.Native).ConnectionString("DefaultConnection").ToADO();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
                options.UseSqlServer(connectionString);
                base.OnConfiguring(options);
            }
        }

        /// <summary>
        /// Set model structure and relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityContext>(entity =>
            {
                entity.ToTable("ActivityContext", "Activity");
                entity.HasKey(p => p.ActivityContextKey);
                entity.Ignore(p => p.ActivityContextId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
