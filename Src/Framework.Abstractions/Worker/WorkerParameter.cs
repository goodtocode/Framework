using System;
using GoodToCode.Extensions;
using GoodToCode.Framework.Security;
using GoodToCode.Framework.Session;

namespace GoodToCode.Framework.Worker
{
    /// <summary>
    /// Result that passes back failed rules, and return data
    /// </summary>
    /// <typeparam name="TDataIn">Type of data to pass</typeparam>
    public class WorkerParameter<TDataIn> : IWorkerParameter<TDataIn>
    {
        private SessionContext context = new SessionContext();
        
        /// <summary>
        /// Identity of user initiating this process
        /// </summary>
        public SessionContext Context { get { return context; } set { context = value as SessionContext; } }

        // Insist any interface types have a concrete equivalent, especially for serialization
        ISessionContext IWorkerParameter<TDataIn>.Context { get { return context; } set { context = value as SessionContext; } }

        /// <summary>
        /// Data to be returned
        /// </summary>
        public TDataIn DataIn { get; set; } = TypeExtension.InvokeConstructorOrDefault<TDataIn>();
        
        /// <summary>
        /// Force hydration on constructor
        /// </summary>
        public WorkerParameter() : base()
        {
        }

        /// <summary>
        /// Constructor that partially hydrates
        /// </summary>
        public WorkerParameter(TDataIn inputData) : this()
        {
            DataIn = inputData;
        }

        /// <summary>
        /// Constructor that fully hydrates
        /// </summary>
        public WorkerParameter(UserPrincipal principalIdentity, TDataIn inputData) : this(inputData)
        {
            Context.Fill(principalIdentity);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">User, device and app context</param>
        /// <param name="data">Data to send</param>
        public WorkerParameter(ISessionContext context, TDataIn data) : this(data)
        {
            Context.Fill(context);
        }       
    }
}
