using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public class EntityWriter<TEntity> : DbContext, IEntityWriter<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        private TEntity _entity = new TEntity();

        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>        
        public TEntity Entity
        {
            get
            {
                ConfigOptions.EntityData = _entity;
                return _entity;
            }
            private set
            {
                _entity = value;
                ConfigOptions.EntityData = _entity;
            }
        }

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
        public EntityWriter()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityWriter(TEntity entity)
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
        /// Create operation on the object
        /// </summary>
        /// <returns>Object pulled from datastore</returns>
        public async Task<TEntity> CreateAsync()
        {
            if (!CanCreate()) throw new NotSupportedException("CanCreate() == false. This entity can not be created in the datastore. Possible causes: Object already has an Id/Key. Object has no data to persist.");
            if (!Entity.IsValid()) throw new NotSupportedException("IsValid() == false. This entity can not be persisted, it is not valid. Please check the object's IsValid() method for valid requirements.");
            Entity.Key = Entity.Key == Defaults.Guid ? Guid.NewGuid() : Entity.Key; // Required to re-pull data after save

            try
            {
                if (ConfigOptions.CreateStoredProcedure != null)
                {
                    var rowsAffected = await ExecuteSqlCommandAsync(ConfigOptions.CreateStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Entity.Key) Entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data
                }
                else
                {
                    Data.Add(Entity);                    
                    Entry(Entity).State = EntityState.Added;
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
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Create() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public async Task<TEntity> UpdateAsync()
        {
            if (!CanUpdate()) throw new NotSupportedException("CanUpdate() == false. This entity can not be created in the datastore. Possible causes: Object already has an Id/Key. Object has no data to persist.");
            if (!Entity.IsValid()) throw new NotSupportedException("IsValid() == false. This entity can not be persisted, it is not valid. Please check the object's IsValid() method for valid requirements.");
            Entity.Key = Entity.Key == Defaults.Guid ? Guid.NewGuid() : Entity.Key;

            try
            {
                if (ConfigOptions.UpdateStoredProcedure != null)
                {
                    var rowsAffected = await ExecuteSqlCommandAsync(ConfigOptions.UpdateStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Entity.Key) Entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data 
                }
                else
                {
                    Entry(Entity).State = EntityState.Modified;
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
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Create() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        public async Task<TEntity> DeleteAsync()
        {
            if (!CanDelete()) throw new NotSupportedException("CanDelete() == false. This entity can not be created in the datastore. Possible causes: Object already has an Id/Key. Object has no data to persist.");
            if (!Entity.IsValid()) throw new NotSupportedException("IsValid() == false. This entity can not be persisted, it is not valid. Please check the object's IsValid() method for valid requirements.");

            try
            {
                if (ConfigOptions.DeleteStoredProcedure != null)
                {
                    var rowsAffected = await ExecuteSqlCommandAsync(ConfigOptions.DeleteStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Defaults.Guid) Entity.Fill(refreshedEntity); // Re-pull clean object, should be "not found"
                }
                else
                {
                    Entry(Entity).State = EntityState.Deleted;
                    Data.Remove(Entity);
                    await SaveChangesAsync();
                    using (EntityReader<TEntity> reader = new EntityReader<TEntity>())
                    {
                        Entity.Fill(reader.GetByKey(Entity.Key)); // Re-pull clean object, exactly as the DB has stored
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Create() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public virtual async Task<TEntity> SaveAsync()
        {
            if (CanCreate())
                return await CreateAsync();
            else if (CanUpdate())
                return await UpdateAsync();
            else throw new System.InvalidOperationException("Save() requires an entity that has CanCreate() or CanUpdate() return true.");
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

        /// <summary>
        /// Executes stored procedure for specific parameter behavior
        /// Named: @Param1 is used to match parameter with entity data. 
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
        /// Ordinal: @Param1, @Param2 are assigned in ordinal position
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
        /// </summary>
        public async Task<int> ExecuteSqlCommandAsync(StoredProcedure<TEntity> storedProc)
        {
            int returnValue;
            switch (ConfigOptions.ParameterBehavior)
            {
                case ParameterBehaviors.Named:
                    returnValue = await Database.ExecuteSqlCommandAsync(storedProc.ToString());
                    break;
                case ParameterBehaviors.Ordinal:
                default:
                    returnValue = await Database.ExecuteSqlCommandAsync(storedProc.SqlPrefix, storedProc.Parameters.ToArray());
                    break;
            }
            return returnValue;
        }
    }
}
