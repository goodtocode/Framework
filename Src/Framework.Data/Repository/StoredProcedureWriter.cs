using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class StoredProcedureWriter<TEntity> : DbContext,
        ICreateOperation<TEntity>, IUpdateOperation<TEntity>, ISaveOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : StoredProcedureEntity<TEntity>, new()
    {
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
        /// Constructor
        /// </summary>
        public StoredProcedureWriter() : base()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public StoredProcedureWriter(DbContextOptions<StoredProcedureWriter<TEntity>> options) : base(options) { }

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
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanInsert(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (entity.IsNew && DatabaseConfig.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
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
            if (!entity.IsNew && DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess)
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
            if (!entity.IsNew && (DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess || DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.NoUpdate))
                returnValue = true;
            return returnValue;
        }
        /// <summary>
        /// Retrieve TEntity objects operation
        /// Default: Does Not read from a Get stored procedure.
        ///  Reads directly from DbSet defined in repository class. 
        /// </summary>
        /// <param name="expression">Expression to query the datastore</param>
        /// <returns>IQueryable of read operation</returns>
        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression)
        {
            return Data.Where(expression);
        }

        /// <summary>
        /// Create operation on the object
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>Object pulled from datastore</returns>
        public TEntity Create(TEntity entity)
        {
            try
            {
                if (entity.CreateStoredProcedure == null) throw new Exception("Create() requires CreateStoredProcedure to be initialized properly.");
                if (entity.IsValid() && CanInsert(entity))
                {
                    if (entity.CreateStoredProcedure.Parameters.Where(x => x.ParameterName == "@Key").Any())
                        entity.Key = entity.Key == Defaults.Guid ? Guid.NewGuid() : entity.Key; // To re-pull data after save
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.CreateStoredProcedure);
                    var refreshedEntity = Read(x => x.Key == entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == entity.Key) entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Create() on {GetType().ToStringSafe()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public TEntity Update(TEntity entity)
        {
            try
            {
                if (entity.UpdateStoredProcedure == null) throw new Exception("Update() requires UpdateStoredProcedure to be initialized properly.");
                if (entity.IsValid() && CanUpdate(entity))
                {
                    entity.Key = entity.Key == Defaults.Guid ? Guid.NewGuid() : entity.Key;
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.UpdateStoredProcedure);
                    var refreshedEntity = Read(x => x.Key == entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == entity.Key) entity.Fill(refreshedEntity); // Re-pull clean object, the DB is allowed to alter data
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Update() on {this.ToString()}");
                throw;
            }

            return entity;
        }

        /// <summary>
        /// Worker that saves this object with automatic tracking.
        /// </summary>
        public virtual TEntity Save(TEntity entity)
        {
            if (CanInsert(entity))
                entity = Create(entity);
            else if (CanUpdate(entity))
                entity = Update(entity);
            return entity;
        }

        /// <summary>
        /// Deletes operation on this entity
        /// </summary>
        public TEntity Delete(TEntity entity)
        {
            try
            {
                if (entity.DeleteStoredProcedure == null) throw new Exception("Delete() requires DeleteStoredProcedure to be initialized properly.");
                if (CanDelete(entity))
                {
                    entity.ActivityContextKey = entity.ActivityContextKey == Defaults.Guid ? ActivityContextWriter.Create().ActivityContextKey : entity.ActivityContextKey;
                    var rowsAffected = ExecuteSqlCommand(entity.DeleteStoredProcedure);
                    var refreshedEntity = Read(x => x.Key == entity.Key).FirstOrDefaultSafe();
                    if (rowsAffected > 0 && refreshedEntity.Key == Defaults.Guid) entity.Fill(refreshedEntity); // Re-pull clean object, should be "not found"
                }
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TEntity), $"StoredProcedureWriter.Delete() on {GetType().ToStringSafe()}");
                throw;
            }

            return entity;
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
        public int ExecuteSqlCommand(StoredProcedure<TEntity> storedProc)
        {
            var returnValue = Defaults.Integer;
            switch (ParameterBehavior)
            {
                case ParameterBehaviors.Named:
                    returnValue = Database.ExecuteSqlCommand(storedProc.ToString());
                    break;
                case ParameterBehaviors.Ordinal:
                default:
                    returnValue = Database.ExecuteSqlCommand(storedProc.SqlPrefix, storedProc.Parameters.ToArray());
                    break;
            }
            return returnValue;
        }
    }
}
