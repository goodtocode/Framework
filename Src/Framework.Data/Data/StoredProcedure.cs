using GoodToCode.Extensions;
using GoodToCode.Framework.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Stored Procedure for entity class
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class StoredProcedure<TEntity> where TEntity : EntityBase<TEntity>, new()
    {
        private string sqlStatement = Defaults.String;
        private string storedProcedureName = Defaults.String;
        private IEnumerable<SqlParameter> sqlParameters = new List<SqlParameter>();

        /// <summary>
        /// Schema to be used for this object's data access
        /// </summary>
        public string DatabaseSchema { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Name of the stored procedure to execute
        /// </summary>
        public string StoredProcedureName
        {
            get => storedProcedureName;
            set
            {
                storedProcedureName = value;
                GenerateSqlStatement();
            }
        }

        /// <summary>
        /// Parameter array for stored procedure
        /// I.e. { new SqlParameter("fromDate", fromDate), new SqlParameter("toDate", toDate) }
        /// </summary>
        public IEnumerable<SqlParameter> Parameters
        {
            get => sqlParameters;
            set => MergeParameters(value);
        }

        /// <summary>
        /// Represents the prefix to the EF call .FromSql(tSqlPrefix, parameters)
        ///  Parameter names are included.
        ///  Parameter values are not included.
        ///  I.e. "EXECUTE MySchema.MySPName @Param1 @Param2"
        /// </summary>
        public string SqlPrefix { get; private set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoredProcedure() : base()
        {
            DatabaseSchema = new TEntity().GetAttributeValue<DatabaseSchemaName>(DatabaseSchema);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="storedProcedure">I.e. "RecordsByDateRange"</param>
        public StoredProcedure(string storedProcedure) : this()
        {
            StoredProcedureName = storedProcedure;
        }

        /// <summary>
        /// Automatically constructs T-SQL based on passed names and objects
        /// </summary>
        /// <param name="storedPrococedure">I.e. "RecordsByDateRange"</param>
        /// <param name="parameters">I.e. { new SqlParameter("fromDate", fromDate), new SqlParameter("toDate", toDate) }</param>
        public StoredProcedure(string storedPrococedure, IEnumerable<SqlParameter> parameters) 
            : this(storedPrococedure)
        {
            sqlParameters = parameters;
        }

        /// <summary>
        /// Merges a range of parameters
        /// </summary>
        /// <param name="newParams"></param>
        public void MergeParameters(IEnumerable<SqlParameter> newParams)
        {
            var dict = Parameters.ToDictionary(p => p.ParameterName);
            foreach (var item in newParams)
                dict[item.ParameterName] = item;
            sqlParameters = dict.Values.ToList();
            GenerateSqlStatement();
        }

        /// <summary>
        /// Rebuilds sqlPrefix and sqlStatement private fields
        /// </summary>
        /// <returns>Fully-formed sqlStatement value</returns>
        private string GenerateSqlStatement()
        {
            SqlPrefix = $"EXECUTE {DatabaseSchema}.{StoredProcedureName}";
            var sqlStatement = SqlPrefix;
            foreach (var item in Parameters)
            {
                var commaSeparator = sqlStatement.Contains("@") ? "," : string.Empty;
                var paramName = item.ParameterName.RemoveFirst("@");
                SqlPrefix = $"{SqlPrefix}{commaSeparator} @{paramName}";
                var quote = RequiresQuote(item.SqlDbType) ? "'" : string.Empty;
                sqlStatement = $"{sqlStatement}{commaSeparator} @{paramName} = {quote}{item.Value}{quote}";
            }
            return sqlStatement;
        }
        
        /// <summary>
        /// T-SQL string to execute.
        /// I.e. "EXECUTE dbo.RecordsByDateRange @fromDate, @toDate"
        /// </summary>
        public override string ToString()
        {
            if (sqlStatement == Defaults.String)
                sqlStatement = GenerateSqlStatement();
            return sqlStatement;
        }

        /// <summary>
        /// Determines if requires quote for SQL Statement
        /// </summary>
        /// <param name="type">SqlDbType to evaluate</param>
        /// <returns>True if requires a single quote (')</returns>
        private bool RequiresQuote(SqlDbType type)
        {
            return type == SqlDbType.BigInt && type == SqlDbType.Bit
                    && type == SqlDbType.Decimal && type == SqlDbType.Float
                    && type == SqlDbType.Int && type == SqlDbType.Money
                    && type == SqlDbType.Real && type == SqlDbType.SmallInt
                    && type == SqlDbType.SmallMoney && type == SqlDbType.Structured
                    && type == SqlDbType.SmallInt && type == SqlDbType.Time
                    && type == SqlDbType.Timestamp && type == SqlDbType.TinyInt
                  ? false : true;
        }
    }
}