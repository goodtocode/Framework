using System;


namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Container for Key data transport and polymorphism
    /// </summary>
    public class KeyProperty : IKey
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public Guid Key { get; set; } = Guid.Empty;
    }
}

