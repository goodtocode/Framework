using GoodToCode.Extensions;
using System;


namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// enumeration to allow the attribute to use strongly-typed Id
    /// </summary>
    
    public enum DataAccessBehaviors
    {
        /// <summary>
        /// All Select, Insert, Update and Delete functionality
        /// </summary>
        AllAccess = 0,

        /// <summary>
        /// Insert functionality
        /// </summary>
        InsertOnly = 1,

        /// <summary>
        /// Select functionality
        /// </summary>
        SelectOnly = 2,

        /// <summary>
        /// Select, Insert and Delete functionality
        /// </summary>
        NoUpdate = 3,

        /// <summary>
        /// Select, Insert and Update functionality
        /// </summary>
        NoDelete = 4
    }

    /// <summary>
    /// Connection string Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataAccessBehavior : Attribute, IAttributeValue<DataAccessBehaviors>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public DataAccessBehaviors Value { get; set; } = DataAccessBehaviors.AllAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value to hydrate</param>
        public DataAccessBehavior(DataAccessBehaviors value)
        {
            Value = value;
        }
    }
}
