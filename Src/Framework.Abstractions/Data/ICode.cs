using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Code, short character-based identifier of a record. 
    ///  Not always ISO codes, sometimes custom or a legacy systems identifier
    /// </summary>
    public interface ICode
    {
        /// <summary>
        /// Code
        /// </summary>
        string Code { get; set; }
    }
}
