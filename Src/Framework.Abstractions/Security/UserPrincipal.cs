
using System;
using System.Security.Principal;
using GoodToCode.Extensions;
using GoodToCode.Framework.Device;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Security
{
    /// <summary>
    /// User Identity based on IPrincipal and IIdentity
    /// </summary>
    public class UserPrincipal : IUserPrincipal, IEntityKey, IDeviceUuid, IApplicationUuid
    {
        private string principalUserName = Defaults.String;

        /// <summary>
        /// Universally Unique Id (UuId) of the device. Typically same as IMEI number, or DeviceId from the OS
        /// </summary>
        public string DeviceUuid { get; set; } = Defaults.String;

        /// <summary>
        /// Universally Unique Id (UuId) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUuid { get; set; } = Defaults.String;

        /// <summary>
        /// Person/business submitting the request
        /// </summary>
        public Guid EntityKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Same as IdentityUserName
        /// User running process is IPrincipal.Name
        /// User logged in is IIdentity.Name
        /// </summary>
        public string IdentityUserName
        {
            get
            {
                principalUserName = principalUserName == Defaults.String ? this.Identity.Name : principalUserName;
                return principalUserName;
            }
            set { principalUserName = value; }
        }

        /// <summary>
        /// User running process is IPrincipal.Name
        /// User logged in is IIdentity.Name
        /// </summary>
        public string PrincipalUserName
        {
            get
            {
                principalUserName = principalUserName == Defaults.String ? this.Identity.Name : principalUserName;
                return principalUserName;
            }
            set { principalUserName = value; }
        }

        /// <summary>
        /// Activity tracking record of this session flow
        /// </summary>
        public int ActivitySessionflowId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Identity of requester
        /// </summary>
        public IIdentity Identity { get; set; } = new UserIdentity();
        
        /// <summary>
        /// Constructor
        /// </summary> 
        public UserPrincipal() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceUuid">Device requesting</param>
        /// <param name="identity">Identity of user of request</param>
        protected UserPrincipal(string deviceUuid, IIdentity identity)
            : base()
        {
            if (identity == null)
                throw new ArgumentNullException("IIdentity");
            DeviceUuid = deviceUuid;
            Identity = identity;
        }
        
        /// <summary>
        /// Is In Role?
        /// </summary>
        /// <param name="role"></param>        
        public bool IsInRole(string role)
        {
            if ("Admin".Equals(role))
                return true;

            throw new ArgumentException("Role " + role + " is not supported");
        }
        
    }
}
