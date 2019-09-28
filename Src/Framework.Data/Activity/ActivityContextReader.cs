using System;
using System.Linq;
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Data context for ActivityContext class
    /// </summary>
    public class ActivityContextReader : DbContext
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<ActivityContext> Data { get; set; }

        /// <summary>
        /// Constructor
        /// Sets |DataDirectory| value for local SQL files (mdf)
        /// </summary>
        public ActivityContextReader() : base()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public ActivityContextReader(DbContextOptions<ActivityContextReader> options) : base(options) { }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        public IQueryable<ActivityContext> GetAll()
        {
            var returnValue = default(IQueryable<ActivityContext>);
            var db = new ActivityContextReader();

            try
            {
                returnValue = db.Data;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(ActivityContext), "ActivityContext.GetAll()");
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        /// <param name="id">The unique Id of the object</param>
        public ActivityContext GetById(int id)
        {
            var returnValue = new ActivityContext();
            var db = new ActivityContextReader();
            try
            {
                returnValue = db.Data.Where(x => x.ActivityContextId == id).FirstOrDefaultSafe();
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(ActivityContext), "ActivityContext.GetById()");
            }

            return returnValue;
        }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        /// <param name="key">The unique Guid Id of the object</param>
        public ActivityContext GetByKey(Guid key)
        {
            var returnValue = new ActivityContext();
            var db = new ActivityContextReader();
            try
            {
                returnValue = db.Data.Where(x => x.ActivityContextKey == key).FirstOrDefaultSafe();
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(ActivityContext), "ActivityContext.GetByKey()");
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
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
