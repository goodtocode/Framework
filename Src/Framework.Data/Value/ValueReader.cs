using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Value
{
    /// <summary>
    /// Reads value objects (unique key style)
    /// </summary>
    public class ValueReader<TValue> : DbContext where TValue : ValueBase<TValue>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<TValue> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IValueReaderConfiguration<TValue> ConfigOptions { get; private set; }

        /// <summary>
        /// Results from any query operation
        /// </summary>
        public IQueryable<TValue> Results { get; protected set; } = default(IQueryable<TValue>);

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
        public ValueReader(string connectionString) : base() { ConfigOptions = new ValueReaderConfiguration<TValue>(connectionString); }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="ignoredTypes"></param>
        public ValueReader(string connectionString, IList<Type> ignoredTypes) : this(connectionString) { ConfigOptions.IgnoredTypes = ignoredTypes; }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="options"></param>
        public ValueReader(string connectionString, DbContextOptions<ValueReader<TValue>> options) : base(options) { ConfigOptions = new ValueReaderConfiguration<TValue>(connectionString); }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TValue> Read(Expression<Func<TValue, Boolean>> whereClause)
        {
            return GetByWhere(whereClause);
        }

        /// <summary>
        /// All data in this datastore subset
        ///  Can add clauses, such as GetAll().Take(1), GetAll().Where(), etc.
        /// </summary>
        public IQueryable<TValue> GetAll()
        {
            Results = Data;

            return Results;
        }

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != -1 And Also Key != Guid.Empty
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        public IQueryable<TValue> GetAllExcludeDefault()
        {
            Results = Data.Where(x => x.Key != Guid.Empty);
            return Results;
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that matches by Key, or an empty entity for not found</returns>
        public TValue GetByKey(Guid key)
        {
            Results = Data.Where(x => x.Key == key);
            return Results.FirstOrDefaultSafe(); ;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TValue> GetByWhere(Expression<Func<TValue, Boolean>> whereClause)
        {
            Results = (whereClause != null) ? Data.Where<TValue>(whereClause) : Data;
            return Results;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <param name="orderByClause">Expression for order by clause</param>
        /// <param name="pageSize">Size of each result</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns></returns>
        public IQueryable<TValue> GetByPage(Expression<Func<TValue, Boolean>> whereClause, Expression<Func<TValue, Boolean>> orderByClause, int pageSize, int pageNumber)
        {
            IQueryable<TValue> returnValue;
            returnValue = (Data).AsQueryable();
            returnValue = (whereClause != null) ? returnValue.Where<TValue>(whereClause).AsQueryable() : returnValue;
            returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
            returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable() : returnValue;
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
