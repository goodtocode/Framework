using System;
using System.Collections.Generic;
using GoodToCode.Extensions.Collections;
using GoodToCode.Framework.Validation;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Result of any process
    /// </summary>
    public interface IWorkerResult
    {
        /// <summary>
        /// CurrentState
        /// </summary>
        WorkerStates CurrentState { get; set; }

        /// <summary>
        /// OnStart
        /// </summary>
        void OnStart();

        /// <summary>
        /// OnSuccess
        /// </summary>
        void OnSuccess();

        /// <summary>
        /// OnFail
        /// </summary>
        /// <param name="errorMessage"></param>
        void OnFail(string errorMessage);

        /// <summary>
        /// FailedRules
        /// </summary>
        List<ValidationResult> FailedRules { get; set; }

        /// <summary>
        /// Return Id - Primary Key of record
        /// </summary>
        int ReturnId { get; set; }

        /// <summary>
        /// Return Key - Guid Key for record
        /// </summary>
        Guid ReturnKey { get; set; }

        /// <summary>
        /// Serialized data to be returned to caller
        /// </summary>
        string ReturnData { get; set; }
    }
}
