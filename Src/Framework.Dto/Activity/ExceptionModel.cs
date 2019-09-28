using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Exception information
    /// </summary>
    /// <remarks></remarks>
    public class ExceptionModel : IExceptionLog
    {
        /// <summary>
        /// Id
        /// </summary>
        public int ExceptionLogId { get; set; } = Defaults.Integer;

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
                } else
                {
                    return Defaults.String;
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
        public string CustomMessage { get; set; } = Defaults.String;

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; } = Defaults.Date;

        /// <summary>
        /// This protected constructor should not be called. Factory methods should be used instead.
        /// </summary>
        public ExceptionModel() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="concreteType"></param>
        /// <param name="customMessage"></param>
        public ExceptionModel(Exception exception, Type concreteType, string customMessage)
            : this()
        {
            CurrentException = exception;
            CustomMessage = $"Error in type: {concreteType.ToString()}. Message: {customMessage}";
        }

        /// <summary>
        /// Returns the typed string of the primary property.
        /// </summary>
        public override string ToString()
        {
            return ExceptionLogId.ToString();
        }
    }
}
