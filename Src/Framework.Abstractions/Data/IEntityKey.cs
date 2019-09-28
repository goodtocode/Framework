using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Used for cases where Entity Id needs to be carried over to Flow via interface
    /// </summary>    
    public interface IEntityKey
    {
        /// <summary>
        /// EntityId
        /// </summary>
        Guid EntityKey { get; set; }
    }
}
