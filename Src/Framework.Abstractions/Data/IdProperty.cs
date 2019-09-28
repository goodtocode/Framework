using System;
using GoodToCode.Extensions;


namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Container for Id data transport and polymorphism
    /// </summary>
    public class IdProperty : IId
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int Id { get; set; } = Defaults.Integer;
    }
}

