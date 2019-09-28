using System;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// States of a an operation that does work
    /// </summary>
    public enum WorkerStates
    {
        /// <summary>
        /// Process never executed
        /// </summary>
        NeverRan = 0x0,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        Pending = 0x1,

        /// <summary>
        /// Process is currently running
        /// </summary>
        Running = 0x2,

        /// <summary>
        /// Process is pending execution
        /// </summary>
        OnHold = 0x4,

        /// <summary>
        /// Process completed with no errors
        /// </summary>
        Completed = 0x8,
    }
}
