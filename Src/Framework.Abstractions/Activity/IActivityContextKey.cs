using System;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Activity that tracks any interaction with the framework
    /// Particularly CRUD and Workflow operations.
    /// </summary>
    public interface IActivityContextKey
    {
        /// <summary>
        /// Key of the activity that tracks a transaction type process, typically querying or committing data
        /// </summary>
        Guid ActivityContextKey { get; set; }
    }
}
