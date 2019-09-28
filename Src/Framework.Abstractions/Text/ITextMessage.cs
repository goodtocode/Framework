using System;

namespace GoodToCode.Framework.Text
{
    /// <summary>
    /// Text interface
    /// </summary>
    public interface ITextMessage
    {
        /// <summary>
        /// LanguageISO
        /// </summary>
        string LanguageIso { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        string Message { get; set; }
    }
}
