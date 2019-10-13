using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// enumeration to allow the attribute to use strongly-typed Id
    /// </summary>    
    public enum Fields
    {
        /// <summary>
        /// Value object: Record has no single identity field
        /// </summary>
        None = -1,

        /// <summary>
        /// Either Id or Key drives record identity
        /// </summary>
        IdOrKey = 0,

        /// <summary>
        /// Id fields defines identity
        /// </summary>
        Id = 1,

        /// <summary>
        /// Key field defines identity
        /// </summary>
        Key = 2
    }

    /// <summary>
    /// Connection string Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RecordIdentity : Attribute, IAttributeValue<Fields>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public Fields Value { get; set; } = Fields.IdOrKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value">Value to hydrate</param>
        public RecordIdentity(Fields value)
        {
            Value = value;
        }
    }
}
