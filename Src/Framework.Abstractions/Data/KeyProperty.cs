using System;
using GoodToCode.Extensions;

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
        public Guid Key { get; set; } = Defaults.Guid;
    }
}

