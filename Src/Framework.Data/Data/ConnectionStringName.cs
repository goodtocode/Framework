using GoodToCode.Extensions;
using System;


namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Class attribute decoration that holds the ConnectionStringName 
    /// Name is the key used to lookup connection string from config file.
    /// </summary>    
    [AttributeUsage(AttributeTargets.Class)]
    public class ConnectionStringName : Attribute, IAttributeValue<string>
    {
        /// <summary>
        /// Default database connection string name
        /// </summary>
        public const string DefaultConnectionName = "DefaultConnection";

        /// <summary>
        /// Name supplied by attribute.
        /// Default is DefaultConnection
        /// </summary>
        public string Value { get; set; } = DefaultConnectionName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString">Connection string name</param>
        public ConnectionStringName(string connectionString)
        {
            Value = connectionString;
        }
    }
}
