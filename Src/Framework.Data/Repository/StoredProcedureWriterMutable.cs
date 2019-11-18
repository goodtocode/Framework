using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using GoodToCode.Framework.Operation;
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
    public class StoredProcedureWriterMutable<TEntity> : DbContext,
        ISaveMutableAsync<TEntity>, ICreateMutableAsync<TEntity>, IUpdateMutableAsync<TEntity>, IDeleteMutableAsync<TEntity>
        where TEntity : EntityBase<TEntity>, new()
    {
        private TEntity _entity = new TEntity();

        /// <summary>
        /// Configures stored procedure for specific parameter behavior
        /// Named: @Param1 is used to match parameter with entity data. 
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
        /// Ordinal: @Param1, @Param2 are assigned in ordinal position
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
        /// </summary>
        public enum ParameterBehaviors
        {
            /// <summary>
            /// Named parameter matching behavior
            ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
            /// </summary>
            Named = 0x0,

            /// <summary>
            /// Ordinal position parameter behavior
            ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
            /// </summary>
            Ordinal = 0x2
        }

        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>
        public TEntity Entity
        {
            get => _entity;
            private set
            {
                _entity = value;
                StoredProcConfig.Entity = _entity;
            }
        }

        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        ///     Note: EF requires public get/set
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ParameterBehaviors ParameterBehavior { get; set; } = ParameterBehaviors.Named;

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IEntityConfiguration<TEntity> DatabaseConfig { get; set; } = new EntityConfiguration<TEntity>();

        /// <summary>
        /// Stored procedure that C-UDs the entity
        /// </summary>
        public IStoredProcedureConfiguration<TEntity> StoredProcConfig { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public StoredProcedureWriterMutable(IStoredProcedureConfiguration<TEntity> storedProcConfig)
        {
            StoredProcConfig = storedProcConfig;
        }

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = Defaults.Boolean;
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanCreate(TEntity entity)
        {
            Entity = entity;
            var returnValue = Defaults.Boolean;
            if (Entity.IsNew && DatabaseConfig.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <returns>True if rules and setup allow for update, else false</returns>
        public bool CanUpdate(TEntity entity)
        {
            Entity = entity;
            var returnValue = Defaults.Boolean;
            if (!Entity.IsNew && DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        public bool CanDelete(TEntity entity)
        {
            Entity = entity;
            var returnValue = Defaults.Boolean;
            if (!Entity.IsNew && (DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess || DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.NoUpdate))
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <returns>Object pulled from datastore</returns>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            Entity = entity;
            try
            {
                if (StoredProcConfig.CreateStoredProcedure == null) throw new ArgumentNullException("Create() requires CreateStoredProcedure to be initialized properly.");
                if (Entity.IsValid() && CanCreate(entity))
                {
                    if (StoredProcConfig.CreateStoredProcedure.Parameters.Where(x => x.ParameterName == "@Key").Any())
                        Entity.Key = Entity.Key == Defaults.Guid ? Guid.NewGuid() : Entity.Key; // To re-pull data after save
                    var rowsAffected = await ExecuteSqlCommandAsync(StoredProcConfig.CreateStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Entity.Key) Entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriterMutable.Create() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Entity = entity;
            try
            {
                if (StoredProcConfig.UpdateStoredProcedure == null) throw new ArgumentNullException("Update() requires UpdateStoredProcedure to be initialized properly.");
                if (Entity.IsValid() && CanUpdate(entity))
                {
                    Entity.Key = Entity.Key == Defaults.Guid ? Guid.NewGuid() : Entity.Key;
                    var rowsAffected = await ExecuteSqlCommandAsync(StoredProcConfig.UpdateStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Entity.Key) Entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriterMutable.Update() on {this.ToString()}");
                throw;
            }

            return Entity;
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public virtual async Task<TEntity> SaveAsync(TEntity entity)
        {
            if (CanCreate(entity))
                return await CreateAsync(entity);
            else if (CanUpdate(entity))
                return await UpdateAsync(entity);
            else throw new System.InvalidOperationException("Save() requires an entity that has CanCreate() or CanUpdate() return true.");
        }

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        public async Task<TEntity> DeleteAsync(TEntity entity)
        {
            Entity = entity;
            try
            {
                if (StoredProcConfig.DeleteStoredProcedure == null) throw new ArgumentNullException("Delete() requires DeleteStoredProcedure to be initialized properly.");
                if (CanDelete(entity))
                {
                    var rowsAffected = await ExecuteSqlCommandAsync(StoredProcConfig.DeleteStoredProcedure);
                    var refreshedEntity = Data.Where(x => x.Key == Entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Defaults.Guid) Entity.Fill(refreshedEntity); // Re-pull clean object, should be "not found"
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriterMutable.Delete() on {GetType().ToStringSafe()}");
                throw;
            }

            return Entity;
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
                if ((DatabaseConfig.ConnectionString.Length == 0 || !CanConnect))
                    throw new Exception("Database connection failed or the connection string could not be found. A valid connection string required for data access.");
                options.UseSqlServer(DatabaseConfig.ConnectionString);
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
            modelBuilder.ApplyConfiguration(DatabaseConfig);
            foreach (Type item in DatabaseConfig.IgnoredTypes)
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
            switch (ParameterBehavior)
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