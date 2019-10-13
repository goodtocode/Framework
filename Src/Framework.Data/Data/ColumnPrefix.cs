using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Class attribute decoration that holds the ColumnPrefix 
    /// If not specified, will be: var ColumnPrefix = typeof(Class).Name
    /// </summary>    
    [AttributeUsage(AttributeTargets.Class)]
    public class ColumnPrefix : Attribute, IAttributeValue<string>
    {
        /// <summary>
        /// Name supplied by attribute. 
        /// Default is DefaultConnection
        /// </summary>
        public string Value { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ColumnPrefix">Database schema name</param>
        public ColumnPrefix(string ColumnPrefix)
        {
            Value = ColumnPrefix;
        }
    }
}
