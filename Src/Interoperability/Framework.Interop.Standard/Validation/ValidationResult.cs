//-----------------------------------------------------------------------
// <copyright file="ValidationResult.cs" company="GoodToCode">
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
using GoodToCode.Extras.Collections;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// class containing basics of a ValidationRule, used to pull out result data from a ValidationRule
    /// </summary>
    public class ValidationResult : KeyValuePairString
    {
        /// <summary>
        /// Language to localize messages
        /// </summary>
        public string LanguageISO { get { return base.Key; } set { base.Key = value; } } 

        /// <summary>
        /// Validation message (localized)
        /// </summary>
        public string Message { get { return base.Value; } set { base.Value = value; } }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <param name="languageISO">Language of message</param>
        public ValidationResult(string message, string languageISO = "eng") : base() { Message = message; this.LanguageISO = languageISO; }
        
    }
}
