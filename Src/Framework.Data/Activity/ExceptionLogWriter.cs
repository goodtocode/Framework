using System;

using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Data context for ExceptionLog class
    /// </summary>
    public class ExceptionLogWriter : DbContext
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<ExceptionLog> Data { get; set; }

        /// <summary>
        /// Constructor
        /// Sets |DataDirectory| value for local SQL files (mdf)
        /// </summary>
        public ExceptionLogWriter() : base()
        {
            
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public ExceptionLogWriter(DbContextOptions<ExceptionLogWriter> options) : base(options) { }

        /// <summary>
        /// Hydrates object and saves the log record
        /// </summary>
        /// <param name="exception">System.Exception object to log</param>
        /// <param name="concreteType">Type that is logging the exception</param>
        /// <param name="customMessage">Custom message to append to the exception log</param>
        /// <returns>Id if successfull, -1 if not.</returns>
        public static ExceptionLog Create(Exception exception, Type concreteType, string customMessage)
        {
            var db = new ExceptionLogWriter();
            var returnValue = new ExceptionLog(exception, concreteType, customMessage) { };
            returnValue = db.Save(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Saves object to database
        /// Silent errors,, Activity is an 100% uptime namespace
        /// </summary>
        public virtual ExceptionLog Save(ExceptionLog entity)
        {
            var returnValue = entity;

            if (entity.ExceptionLogId == -1)
            {
                Database.EnsureCreated();
                entity.ExceptionLogKey = entity.ExceptionLogKey == Guid.Empty ? Guid.NewGuid() : entity.ExceptionLogKey;
                Entry(entity).State = EntityState.Added;                
                SaveChanges();
                returnValue = new ExceptionLogReader().GetByKey(entity.ExceptionLogKey);
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
            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.ToTable("ExceptionLog", "Activity");
                entity.HasKey(p => p.ExceptionLogKey);
                entity.Ignore(p => p.ExceptionLogId);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}