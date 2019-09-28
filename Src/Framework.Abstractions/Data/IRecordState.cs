using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Base used for all Value classes
    /// </summary>
    public interface IRecordState
    {
        /// <summary>
        /// Status of this record with static values: 0x0 - Default, 0x1 - ReadOnly, 0x2 - Historical, 0x4 - Deleted
        /// </summary>
        Guid State { get; set; }
    }
}
