using System;
using System.Collections.Generic;
using GoodToCode.Extensions.Collections;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// class containing basics of a ValidationRule, used to pull out result data from a ValidationRule
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        /// <summary>
        /// Language to localize messages
        /// </summary>
        public string LanguageISO { get; set; } 

        /// <summary>
        /// Validation message (localized)
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message to add</param>
        /// <param name="languageISO">Language of message</param>
        public ValidationResult(string message, string languageISO = "eng") { Message = message; this.LanguageISO = languageISO; }
        
    }
}
