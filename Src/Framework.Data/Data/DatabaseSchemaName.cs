using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Class attribute decoration that holds the DatabaseSchemaName 
    /// Name is the key used to lookup connection string from config file.
    /// </summary>    
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseSchemaName : Attribute, IAttributeValue<string>
    {
        /// <summary>
        /// Default Code Schema name
        /// </summary>
        public const string DefaultDatabaseSchema = "dbo";

        /// <summary>
        /// Default Activity Schema name
        /// </summary>
        public const string DefaultActivitySchema = "Activity";

        /// <summary>
        /// Name supplied by attribute. 
        /// Default is DefaultConnection
        /// </summary>
        public string Value { get; set; } = DatabaseSchemaName.DefaultDatabaseSchema;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databaseSchema">Database schema name</param>
        public DatabaseSchemaName(string databaseSchema)
        {
            Value = databaseSchema;
        }
    }
}
