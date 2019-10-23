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
