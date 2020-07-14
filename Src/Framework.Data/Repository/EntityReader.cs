
using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using GoodToCode.Framework.Operation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public class EntityReader<TEntity> : DbContext, IEntityReader<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IEntityReaderConfiguration<TEntity> ConfigOptions { get; private set; }

        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<TEntity> Data { get; set; }

        /// <summary>
        /// Results from any query operation
        /// </summary>
        public IQueryable<TEntity> Results { get; protected set; } = default(IQueryable<TEntity>);

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = false;
                using (var connection = new SqlConnection(ConfigOptions.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="connectionString"></param>
        public EntityReader(string connectionString) : base() { ConfigOptions = new EntityReaderConfiguration<TEntity>(connectionString); }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredTypes"></param>
        public EntityReader(string connectionString, IList<Type> ignoredTypes) : this(connectionString) { ConfigOptions.IgnoredTypes = ignoredTypes; }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="options"></param>
        public EntityReader(string connectionString, DbContextOptions<EntityReader<TEntity>> options) : base(options) { ConfigOptions = new EntityReaderConfiguration<TEntity>(connectionString); }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TEntity> Read(Expression<Func<TEntity, Boolean>> whereClause)
        {
            return GetByWhere(whereClause);
        }

        /// <summary>
        /// All data in this datastore subset
        ///  Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public IQueryable<TEntity> GetAll()
        {
            Results = Data;
            return Results;
        }

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != -1 And Also Key != Guid.Empty
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        public IQueryable<TEntity> GetAllExcludeDefault()
        {
            Results = Data.Where(x => x.Id != -1 && x.Key != Guid.Empty);
            return Results;
        }

        /// <summary>
        /// Gets one or no items based on exact ID or Key match
        ///   Id used if value entered is of type int
        ///   Key used if value passed is of type Guid
        /// </summary>
        /// <returns>One or no TEntity based on exact Key match</returns>
        public TEntity GetByIdOrKey(string idOrKey)
        {
            if (idOrKey.IsInteger())
                return GetById(idOrKey.TryParseInt32());
            else
                return GetByKey(idOrKey.TryParseGuid());
        }

        /// <summary>
        /// Gets database record with exact Id match
        /// </summary>
        /// <param name="id">Database Id of the record to pull</param>
        /// <returns>Single entity that matches by id, or an empty entity for not found</returns>
        public TEntity GetById(int id)
        {
            Results = Data.Where(x => x.Id == id);
            return Results.FirstOrDefaultSafe();
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that matches by Key, or an empty entity for not found</returns>
        public TEntity GetByKey(Guid key)
        {
            Results = Data.Where(x => x.Key == key);
            return Results.FirstOrDefaultSafe(); ;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, Boolean>> whereClause)
        {
            Results = (whereClause != null) ? Data.Where<TEntity>(whereClause) : Data;
            return Results;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <param name="orderByClause">Expression for order by clause</param>
        /// <param name="pageSize">Size of each result</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>Page of data, based on passed clauses and page parameters</returns>
        public IQueryable<TEntity> GetByPage(Expression<Func<TEntity, bool>> whereClause, Expression<Func<TEntity, object>> orderByClause, int pageSize, int pageNumber)
        {
            IQueryable<TEntity> returnValue;
            returnValue = (Data).AsQueryable();
            returnValue = (whereClause != null) ? returnValue.Where<TEntity>(whereClause).AsQueryable() : returnValue;
            returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
            returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip(((pageNumber - 1) * pageSize)).Take(pageSize).AsQueryable() : returnValue;
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
                modelBuilder.Ignore(item);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
