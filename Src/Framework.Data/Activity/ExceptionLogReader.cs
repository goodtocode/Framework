using System;
using System.Linq;
using GoodToCode.Extensions;
using GoodToCode.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Data context for ExceptionLog class
    /// </summary>
    public class ExceptionLogReader : DbContext
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<ExceptionLog> Data { get; set; }

        /// <summary>
        /// Constructor
        /// Sets |DataDirectory| value for local SQL files (mdf)
        /// </summary>
        public ExceptionLogReader() : base()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public ExceptionLogReader(DbContextOptions<ExceptionLogReader> options) : base(options) { }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        public IQueryable<ExceptionLog> GetAll()
        {
            return Data;
        }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        /// <param name="id">The unique Id of the object</param>
        public ExceptionLog GetById(int id)
        {
            var returnValue = new ExceptionLog();
            var db = new ExceptionLogReader();
            returnValue = db.Data.Where(x => x.ExceptionLogId == id).FirstOrDefaultSafe();
            return returnValue;
        }

        /// <summary>
        /// Loads an existing object based on Id.
        /// </summary>
        /// <param name="key">The unique Guid Id of the object</param>
        public ExceptionLog GetByKey(Guid key)
        {
            var returnValue = new ExceptionLog();
            var db = new ExceptionLogReader();
            returnValue = db.Data.Where(x => x.ExceptionLogKey == key).FirstOrDefaultSafe();
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
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
