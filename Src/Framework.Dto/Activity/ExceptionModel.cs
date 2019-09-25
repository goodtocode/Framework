//-----------------------------------------------------------------------
// <copyright file="ExceptionModel.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
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
