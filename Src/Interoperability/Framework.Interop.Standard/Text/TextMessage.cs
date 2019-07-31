//-----------------------------------------------------------------------
// <copyright file="IText.cs" company="GoodToCode">
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
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Text
{
    /// <summary>
    /// Text interface
    /// </summary>
    public class TextMessage : ITextMessage
    {
        /// <summary>
        /// LanguageISO
        /// </summary>
        public string LanguageIso { get; set; } = Defaults.String;

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public TextMessage()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="languageIsoCode"></param>
        public TextMessage(string message, string languageIsoCode = "en-US")
        {
            Message = message;
            LanguageIso = languageIsoCode;
        }
    }
}
