namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// Validation Rule contract
    /// </summary>
    public interface IValidationResult
    {
        /// <summary>
        /// Language to localize messages
        /// </summary>
        string LanguageISO { get; set; }

        /// <summary>
        /// Validation message (localized)
        /// </summary>
        string Message { get; set; }
    }    
}
