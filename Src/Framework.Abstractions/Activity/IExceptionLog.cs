using System;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Exception logged record
    /// </summary>
    public interface IExceptionLog : ICreatedDate
    {
        /// <summary>
        /// Custom message from exception
        /// </summary>
        string CustomMessage { get; set; }

        /// <summary>
        /// Full exception message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Inner exception
        /// </summary>
        string InnerException { get; }

        /// <summary>
        /// Stack trace of the exception
        /// </summary>
        string StackTrace { get; }
    }
}
