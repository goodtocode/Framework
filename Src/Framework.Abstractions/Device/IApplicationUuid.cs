using System;

namespace GoodToCode.Framework.Device
{
    /// <summary>
    /// Device Id
    /// </summary>
    public interface IApplicationUuid
    {
        /// <summary>
        /// Universally Unique Id (UuId) of the software application, that identifies this Application + Device combination
        /// </summary>
        string ApplicationUuid { get; }
    }
}
