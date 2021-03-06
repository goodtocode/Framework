using System;


namespace GoodToCode.Framework.Session
{
    /// <summary>
    /// Context identity that includes user identity info (user name), application Id and entityID
    /// </summary>
    public class SessionContext : ISessionContext
    {
        /// <summary>
        /// Universally Unique Id (UuId) of the device. Typically same as IMEI number, or DeviceId from the OS
        /// </summary>
        public string DeviceUuid { get; set; } = string.Empty;

        /// <summary>
        /// Universally Unique Id (UuId) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUuid { get; set; } = string.Empty;

        /// <summary>
        /// Entity (business or person)
        /// </summary>
        public Guid EntityKey { get; set; } = Guid.Empty;

        /// <summary>
        /// Name, typically user name
        /// </summary>
        public string IdentityUserName { get; set; } = string.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionContext() : base() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceUuid">Device Id sending request</param>
        /// <param name="applicationUuid">Application Id sending request</param>
        /// <param name="identityUserName">Name of user/authentication name sending request</param>
        public SessionContext(string deviceUuid, string applicationUuid, string identityUserName) : this()
        {
            DeviceUuid = deviceUuid;
            ApplicationUuid = applicationUuid;
            IdentityUserName = identityUserName;            
        }
                
    }
}
