using System;
using GoodToCode.Framework.Session;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Parameter and data for any process
    /// </summary>
    /// <typeparam name="TDataIn">Type of input data for the process</typeparam>
    public interface IWorkerParameter<TDataIn>
    {
        /// <summary>
        /// App, User, Device context
        /// </summary>
        ISessionContext Context { get; set; }

        /// <summary>
        /// Input data for the process
        /// </summary>
        TDataIn DataIn { get; set; }
    }
}
