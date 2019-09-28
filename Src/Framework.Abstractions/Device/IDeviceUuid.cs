using System;

namespace GoodToCode.Framework.Device
{
    /// <summary>
    /// Device Id
    /// </summary>
    public interface IDeviceUuid
    {
        /// <summary>
        /// Universally Unique Id (UuId) of the device, typically the IMEI from the hardware, or DeviceId from the operating system
        /// </summary>
        string DeviceUuid { get; }        
    }
}
