using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Id, used in every object
    /// </summary>    
    public interface IKey
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Key { get; set; }
    }
}
