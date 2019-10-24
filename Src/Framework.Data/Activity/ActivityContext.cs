using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// This data-only object is used by EF code-first to define the schema of the table that log Exceptions
    /// </summary>
    /// <remarks></remarks>
    [DatabaseSchemaName(DatabaseSchemaName.DefaultActivitySchema), ConnectionStringName(ConnectionStringName.DefaultConnectionName)]
    public class ActivityContext : IActivityContext
    {
        private string identityUserName = Defaults.String;
        private string principalIP4Address = Defaults.String;
        private string stackTrace = Defaults.String;
        private string executingContext = Defaults.String;
        private string deviceUuid = Defaults.String;
        private string applicationUuid = Defaults.String;

        /// <summary>
        /// Integer Id of this record
        /// </summary>
        public int ActivityContextId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Guid Id of this record
        /// </summary>
        public Guid ActivityContextKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Date record was created
        /// </summary>
        public DateTime CreatedDate { get; set; } = Defaults.Date;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public DateTime ModifiedDate { get; set; } = Defaults.Date;

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
            get {
                if (executingContext == Defaults.String)
                    executingContext = ExecutingContextInfo();
                return executingContext;
            }
            set { executingContext = value.SubstringLeft(2000); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ActivityContext() : base()
        {
            IdentityUserName = $@"{Environment.UserDomainName}\{Environment.UserName}";
            ExecutingContext = ExecutingContextInfo();
            StackTrace = Environment.StackTrace;
            PrincipalIP4Address = GetIPAddresses().FirstOrDefault().ToString();
        }
        
        /// <summary>
        /// Builds runtime context in the format of: Assembly FQN || Executing location || Machine Name - Domain\User
        /// </summary>
        /// <returns></returns>
        private string ExecutingContextInfo()
        {
            var returnValue = Defaults.String;

            try
            {
                returnValue = $@"{Assembly.GetExecutingAssembly().FullName} || {Assembly.GetExecutingAssembly().Location} || {Environment.MachineName} - {Environment.UserDomainName}\{Environment.UserName}";
            }
            catch
            {
                returnValue = Defaults.String;
            }

            return returnValue;
        }

        /// <summary> 
        /// Retrieves IP addresses for one family (IPv4 or IPv6)
        /// </summary> 
        private List<IPAddress> GetIPAddresses(AddressFamily family = AddressFamily.InterNetwork)
        {
            var returnValue = new List<IPAddress>();
            var sb = new StringBuilder();

            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in interfaces)
            {
                var properties = network.GetIPProperties();
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != family)
                        continue;
                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    returnValue.Add(address.Address);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Returns the typed string of the primary property.
        /// </summary>
        public override string ToString()
        {
            return ActivityContextId.ToString();
        }
    }
}
