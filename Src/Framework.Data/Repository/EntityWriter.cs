using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public class EntityWriter<TEntity> : DbContext, ISaveOperationAsync<TEntity>, IDeleteOperationAsync<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>
        public TEntity Entity { get; } = new TEntity();

        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IEntityConfiguration<TEntity> ConfigOptions { get; set; } = new EntityConfiguration<TEntity>(prop => prop.Id);

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
        public EntityWriter(TEntity entity) : base()
        {
            Entity = entity;
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="entity">entity to persist</param>
        /// <param name="options"></param>
        public EntityWriter(TEntity entity, DbContextOptions<EntityWriter<TEntity>> options) : base(options)
        {
            Entity = entity;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityWriter(TEntity entity, IEntityConfiguration<TEntity> databaseConfig) : this(entity)
        {
            ConfigOptions = databaseConfig;
        }

        /// <summary>
        /// Inserts this object with Workflow-based tracking.
        /// </summary>  
        public async Task<TEntity> CreateAsync()
        {
            return await SaveAsync();
        }

        ///// <summary>
        ///// Inserts this object with Workflow-based tracking.
        ///// </summary>  
        ///// <param name="activity">Activity record owning this process</param>
        //public async Task<TEntity> CreateAsync(IActivityContext activity)
        //{
        //    Entity.ActivityContextKey = activity.ActivityContextKey;
        //    return await SaveAsync();
        //}

        /// <summary>
        /// Updates this object with Workflow-based tracking.
        /// </summary>  
        public async Task<TEntity> UpdateAsync()
        {
            return await SaveAsync();
        }
        ///// <summary>
        ///// Updates this object with Workflow-based tracking.
        ///// </summary>  
        ///// <param name="activity">Activity record owning this process</param>
        //public async Task<TEntity> UpdateAsync(IActivityContext activity)
        //{
        //    Entity.ActivityContextKey = activity.ActivityContextKey;
        //    return await SaveAsync();
        //}

        ///// <summary>
        ///// Inserts or Updates this object with Workflow-based tracking.
        ///// </summary>  
        ///// <param name="activity">Activity record owning this process</param>
        //public async Task<TEntity> DeleteAsync(IActivityContext activity)
        //{
        //    Entity.ActivityContextKey = activity.ActivityContextKey;
        //    return await DeleteAsync();
        //}

        ///// <summary>
        ///// Inserts or Updates this object with Workflow-based tracking.
        ///// </summary>  
        ///// <param name="activity">Activity record owning this process</param>
        //public async Task<TEntity> SaveAsync(IActivityContext activity)
        //{
        //    Entity.ActivityContextKey = activity.ActivityContextKey;
        //    return await SaveAsync();
        //}


        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public async Task<TEntity> SaveAsync()
        {
            var activity = new ActivityContext();
            var trackingState = EntityState.Unchanged;

            try
            {
                Entity.ActivityContextKey = Entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : Entity.ActivityContextKey;
                if (CanCreate())
                {
                    trackingState = EntityState.Added;
                    Data.Add(Entity);
                }
                else if (CanUpdate())
                    trackingState = EntityState.Modified;
                if (Entity.IsValid() && trackingState != EntityState.Unchanged)
                {
                    Entity.Key = Entity.Key == Defaults.Guid ? Guid.NewGuid() : Entity.Key; // Required to re-pull data after save
                    Entry(Entity).State = trackingState;
                    await SaveChangesAsync();
                    using (var reader = new EntityReader<TEntity>())
                    {
                        var refreshedEntity = reader.GetByKey(Entity.Key);
                        Entity.Fill(refreshedEntity); // Re-pull clean object, exactly as the DB has stored
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Save() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Worker that deletes this object with automatic tracking
        /// </summary>      
        /// <returns>True if record deleted, false if not</returns>
        public async Task<TEntity> DeleteAsync()
        {
            try
            {
                if (CanDelete())
                {
                    Entry(Entity).State = EntityState.Deleted;
                    Data.Remove(Entity);
                    await SaveChangesAsync();
                    Entity.Fill(new EntityReader<TEntity>().GetByKey(Entity.Key)); // Re-pull clean object, exactly as the DB has stored
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"EntityWriter.Delete() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanCreate()
        {
            var returnValue = Defaults.Boolean;
            if (Entity.IsNew && ConfigOptions.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <returns>True if rules and setup allow for update, else false</returns>
        public bool CanUpdate()
        {
            var returnValue = Defaults.Boolean;
            if (!Entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        public bool CanDelete()
        {
            var returnValue = Defaults.Boolean;
            if (!Entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
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
