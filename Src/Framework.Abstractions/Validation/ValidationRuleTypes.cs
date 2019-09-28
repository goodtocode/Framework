using System;
using GoodToCode.Extensions;
using GoodToCode.Framework.Text;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// ValidationRuleTypes static values for compile time references without needing runtime data access
    /// </summary>
    public struct ValidationRuleTypes
    {
        /// <summary>
        /// Non destructive warning when validation fails
        /// </summary>
        public static Guid Success = Defaults.Guid;

        /// <summary>
        /// Non destructive warning when validation fails
        /// </summary>
        public static Guid Warning = new Guid("52ED403E-2839-4597-BA8A-6A7C2D8A511B");

        /// <summary>
        /// Failed validation allows saving of data, but record is not completed and in Work In Progress status
        /// </summary>
        public static Guid InProgress = new Guid("A5210E6A-59A1-4F7F-8113-2796F9CE3618");

        /// <summary>
        /// Fatal error condition
        /// </summary>
        public static Guid Error = new Guid("4087CB0C-4951-4E1A-BA1D-F6FF6339D47D");
    }
}
