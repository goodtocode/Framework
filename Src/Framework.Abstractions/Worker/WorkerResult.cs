using System;
using GoodToCode.Extensions.Collections;
using GoodToCode.Framework.Validation;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    public class WorkerResult : IWorkerResult
    {      
        /// <summary>
        /// Current state of the process
        /// </summary>
        public WorkerStates CurrentState { get; set; } = WorkerStates.NeverRan;

        /// <summary>
        /// Errors
        /// </summary>
        /// <value></value>        
        public KeyValueListString FailedRules { get; set; } = new KeyValueListString();

        /// <summary>
        /// Id to be returned to caller
        /// </summary>
        public int ReturnId { get; set; } = -1;

        /// <summary>
        /// Key to be returned to caller
        /// </summary>
        public Guid ReturnKey { get; set; } = Guid.Empty;

        /// <summary>
        /// Serialized data to be returned to caller
        /// </summary>
        public string ReturnData { get; set; } = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public WorkerResult() : base()
        {
        }
        
        /// <summary>
        /// Adds to failed rules from a valid IValidationRule or IValidationResult
        /// </summary>
        /// <param name="validatable"></param>
        public void RuleFailed(ValidationResult validatable)
        {
            FailedRules.Add(validatable);
        }

        /// <summary>
        /// Adds a failed rule message that does not have access to a full IValidationRule or IValidationResult
        /// </summary>
        /// <param name="resultMessageWithNoValidationRule"></param>
        public void RuleFailed(string resultMessageWithNoValidationRule)
        {
            FailedRules.Add(new ValidationResult(resultMessageWithNoValidationRule));
        }
        
        /// <summary>
        /// Starts a process
        /// </summary>        
        public void OnStart()
        {
            CurrentState = WorkerStates.Running;
        }

        /// <summary>
        /// Records failure of a process
        /// </summary>
        public void OnFail(string errorMessage)
        {
            CurrentState = WorkerStates.OnHold;
            RuleFailed(errorMessage);
        }

        /// <summary>
        /// Finalizes a process
        /// </summary>
        public void OnSuccess()
        {
            CurrentState = WorkerStates.Completed;
        }
    }
}
