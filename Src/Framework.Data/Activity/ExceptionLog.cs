using System;
using System.Reflection;
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// This data-only object is used by EF code-first to define the schema of the table that log Exceptions
    /// </summary>
    /// <remarks></remarks>
    [DatabaseSchemaName(DatabaseSchemaName.DefaultActivitySchema)]
    public class ExceptionLog : IExceptionLog
    {
        private Uri endpointUrl = new Uri("http://localhost:80", UriKind.RelativeOrAbsolute);
        private Exception currentException = new System.Exception("No Exception");

        /// <summary>
        /// Integer Id of this record
        /// </summary>
        public int ExceptionLogId { get; set; } = -1;

        /// <summary>
        /// Guid Id of this record
        /// </summary>
        public Guid ExceptionLogKey { get; set; } = Guid.Empty;

        /// <summary>
        /// Date record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);

        /// <summary>
        /// exceptionField
        /// </summary>
        protected Exception CurrentException { get; set; } = new Exception("No exception thrown");

        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return CurrentException.Message; }
            protected set { }
        }

        /// <summary>
        /// InnerException
        /// </summary>
        public string InnerException
        {
            get
            {
                if (CurrentException.InnerException == null == false)
                {
                    return CurrentException.InnerException.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            protected set { }
        }

        /// <summary>
        /// StackTrace
        /// </summary>
        public string StackTrace
        {
            get { return CurrentException.StackTrace.ToStringSafe(); }
            protected set { }
        }

        /// <summary>
        /// CustomMessage
        /// </summary>
        public string CustomMessage { get; set; } = string.Empty;

        /// <summary>
        /// MachineName
        /// </summary>
        public string MachineName { get { return Environment.MachineName; } }

        /// <summary>
        /// ADDomainName
        /// </summary>
        public string ADDomainName { get { return Environment.UserDomainName; } protected set { } }

        /// <summary>
        /// ADUserName
        /// </summary>
        public string ADUserName { get { return Environment.UserName; } protected set { } }

        /// <summary>
        /// DirectoryWorking
        /// </summary>
        public string DirectoryWorking { get { return Environment.CurrentDirectory; } protected set { } }

        /// <summary>
        /// DirectoryAssembly
        /// </summary>
        public string DirectoryAssembly { get { return Assembly.GetExecutingAssembly().Location; } protected set { } }

        /// <summary>
        /// ApplicationName
        /// </summary>
        public string AssemblyName { get { return Assembly.GetExecutingAssembly().FullName; } protected set { } }

        /// <summary>
        /// URL
        /// </summary>
        public string URL { get { return endpointUrl.ToString(); } protected set { } }

        /// <summary>
        /// Contructor
        /// </summary>
        public ExceptionLog() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="concreteType"></param>
        /// <param name="customMessage"></param>
        public ExceptionLog(Exception exception, Type concreteType, string customMessage)
            : this()
        {            
            CurrentException = exception;
            CustomMessage = $"Error in type: {concreteType.ToString()}. Message: {customMessage}";
        }
    }
}
