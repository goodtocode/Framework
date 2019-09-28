using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// This data-only object is used by EF code-first to define the schema of the table that log Exceptions
    /// </summary>
    /// <remarks></remarks>
    public class ActivityModel : IActivityContext
    {
        private string identityUserName = Defaults.String;
        private string principalIP4Address = Defaults.String;
        private string stackTrace = Defaults.String;
        private string executingContext = Defaults.String;
        private string deviceUuid = Defaults.String;
        private string applicationUuid = Defaults.String;

        /// <summary>
        /// Id
        /// </summary>
        public int ActivityContextId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Key
        /// </summary>
        public Guid ActivityContextKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// IdentityUserName
        /// </summary>
        public string IdentityUserName
        {
            get { return identityUserName; }
            set { identityUserName = value.SubstringLeft(40); }
        }

        /// <summary>
        /// Universally Unique Id (UuId) of the device, typically the IMEI from the hardware, or DeviceId from the operating system
        /// </summary>
        public string DeviceUuid
        {
            get { return deviceUuid; }
            set { deviceUuid = value.SubstringLeft(250); }
        }

        /// <summary>
        /// Universally Unique Id (UuId) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUuid
        {
            get { return applicationUuid; }
            set { applicationUuid = value.SubstringLeft(250); }
        }

        /// <summary>
        /// PrincipalIP4Address
        /// </summary>
        public string PrincipalIP4Address
        {
            get { return principalIP4Address; }
            set { principalIP4Address = value.SubstringLeft(15); }
        }

        /// <summary>
        /// StackTrace
        /// </summary>
        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value.SubstringLeft(4000); }
        }

        /// <summary>
        /// Runtime context info, like assembly and environment info
        /// </summary>
        public string ExecutingContext
        {
            get { return executingContext; }
            set { executingContext = value.SubstringLeft(2000); }
        }

        /// <summary>
        /// ModifiedDate
        /// </summary>
        public DateTime ModifiedDate { get; set; } = Defaults.Date;

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; } = Defaults.Date;

        /// <summary>
        /// Returns the typed string of the primary property.
        /// </summary>
        public override string ToString()
        {
            return ActivityContextId.ToString();
        }
    }
}
