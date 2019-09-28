using System;
using GoodToCode.Extensions;


namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Container for Code data transport and polymorphism
    /// </summary>
    public class CodeProperty : ICode
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public string Code { get; set; } = Defaults.String;
    }
}

