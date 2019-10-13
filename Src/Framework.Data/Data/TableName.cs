using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Class attribute decoration that holds the TableName 
    /// If not specified, will be: var tableName = typeof(Class).Name
    /// </summary>    
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Attribute, IAttributeValue<string>
    {
        /// <summary>
        /// Name supplied by attribute. 
        /// Default is DefaultConnection
        /// </summary>
        public string Value { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">Database schema name</param>
        public TableName(string tableName)
        {
            Value = tableName;
        }
    }
}
