using System;
using GoodToCode.Extensions.Collections;

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
