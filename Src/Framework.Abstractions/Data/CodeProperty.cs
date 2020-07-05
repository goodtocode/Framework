using System;



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
        public string Code { get; set; } = string.Empty;
    }
}

