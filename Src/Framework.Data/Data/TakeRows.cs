using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Take Top Rows Attribute
    ///  Default: 100 rows
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TakeRows : Attribute, IAttributeValue<int>
    {
        /// <summary>
        /// Value of attribute
        /// </summary>
        public int Value { get; set; } = 100;

        /// <summary>
        /// Number of rows to take top from query
        ///  Default: 100 rows
        /// </summary>
        /// <param name="value">number of rows to take</param>
        public TakeRows(int value)
        {
            Value = value;
        }
    }
}
