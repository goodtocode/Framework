using GoodToCode.Extensions;
using GoodToCode.Extensions.Data;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class EntityWriter<TEntity> : DbContext, ISaveOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public virtual IEntityConfiguration<TEntity> ConfigOptions { get; set; } = new EntityConfiguration<TEntity>(prop => prop.Id);

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = Defaults.Boolean;
                using (var connection = new SqlConnection(ConfigOptions.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityWriter() : base()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public EntityWriter(DbContextOptions<EntityWriter<TEntity>> options) : base(options) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityWriter(IEntityConfiguration<TEntity> databaseConfig) : this()
        {
            ConfigOptions = databaseConfig;
        }

        /// <summary>
        /// Inserts this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Create(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
        }

        /// <summary>
        /// Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Update(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Delete(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Delete(entity);
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Save(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
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
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Save() on {GetType().ToStringSafe()}");
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
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Delete() on {GetType().ToStringSafe()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanInsert(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (entity.IsNew && ConfigOptions.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <param name="entity">Entity to be updated in the datastore</param>
        /// <returns>True if rules and setup allow for update, else false</returns>
        public bool CanUpdate(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (!entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <param name="entity">Entity to be deleted in the datastore</param>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        public bool CanDelete(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (!entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
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
                if ((ConfigOptions.ConnectionString.Length == 0 || !CanConnect))
                    throw new Exception("Database connection failed or the connection string could not be found. A valid connection string required for data access.");
                options.UseSqlServer(ConfigOptions.ConnectionString);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                base.OnConfiguring(options);
            }
        }

        /// <summary>
        /// Set model structure and relationships
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(ConfigOptions);
            foreach (Type item in ConfigOptions.IgnoredTypes)
            {
                modelBuilder.Ignore(item);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
