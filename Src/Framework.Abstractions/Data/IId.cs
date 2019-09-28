using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Id, used in every object
    /// </summary>    
    public interface IId
    {
        /// <summary>
        /// Id
        /// </summary>
        int Id { get; set; }
    }
}
