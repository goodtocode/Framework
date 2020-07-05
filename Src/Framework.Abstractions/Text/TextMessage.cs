

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
        public string LanguageIso { get; set; } = string.Empty;

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; } = string.Empty;

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
