using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// Workaround 1: EF Core in .NET Standard project: csproj - <AutoGenerateBindingRedirects>true</AutoGenerateBindingRedirects>
    /// Workaround 2: EF Core in test project: csproj - <GenerateBindingRedirectsOutputType>true</GenerateBindingRedirectsOutputType>
    /// </summary>
    public partial class ValueReader<TValue> : DbContext where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Data set DbSet class that gets/saves the entity.
        /// </summary>
        public DbSet<TValue> Data { get; set; }

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IValueConfiguration<TValue> ConfigOptions { get; set; } = new ValueConfiguration<TValue>();

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
        public ValueReader() : base()
        {
            
        }

        /// <summary>
        /// Constuctor for options
        /// </summary>
        /// <param name="options"></param>
        public ValueReader(DbContextOptions<ValueReader<TValue>> options) : base(options) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueReader(IValueConfiguration<TValue> databaseConfig) : this()
        {
            ConfigOptions = databaseConfig;
            
        }

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
            try
            {
                Results = Data;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetAll()");
                throw;
            }

            return Results;
        }

        /// <summary>
        /// All data in this datastore subset, except records with default Id/Key
        ///  Criteria: Where Id != Defaults.Integer And Also Key != Defaults.Guid
        ///  Goal: To exclude "Not Selected" records from lookup tables
        /// </summary>
        public IQueryable<TValue> GetAllExcludeDefault()
        {
            try
            {
                Results = Data.Where(x => x.Key != Defaults.Guid);
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetAllExcludeDefault()");
                throw;
            }

            return Results;
        }

        /// <summary>
        /// Gets database record with exact Key match
        /// </summary>
        /// <param name="key">Database Key of the record to pull</param>
        /// <returns>Single entity that matches by Key, or an empty entity for not found</returns>
        public TValue GetByKey(Guid key)
        {
            try
            {
                Results = Data.Where(x => x.Key == key);
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetByKey()");
                throw;
            }

            return Results.FirstOrDefaultSafe(); ;
        }

        /// <summary>
        /// Retrieves data with purpose of displaying results over multiple pages (i.e. in Grid/table)
        /// </summary>
        /// <param name="whereClause">Expression for where clause</param>
        /// <returns></returns>
        public IQueryable<TValue> GetByWhere(Expression<Func<TValue, Boolean>> whereClause)
        {
            try
            {
                Results = (whereClause != null) ? Data.Where<TValue>(whereClause) : Data;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "ValueReader.GetByWhere()");
                throw;
            }

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
            var returnValue = default(IQueryable<TValue>);

            try
            {
                returnValue = (Data).AsQueryable();
                returnValue = (whereClause != null) ? returnValue.Where<TValue>(whereClause).AsQueryable() : returnValue;
                returnValue = (orderByClause != null) ? returnValue.OrderBy(orderByClause).AsQueryable() : returnValue;
                returnValue = (pageNumber > 0 && pageSize > 0) ? returnValue.Skip((pageNumber * pageSize)).Take(pageSize).AsQueryable() : returnValue;
            }
            catch (Exception ex)
            {
                ExceptionLogWriter.Create(ex, typeof(TValue), "EntityReader.GetByPage()");
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
