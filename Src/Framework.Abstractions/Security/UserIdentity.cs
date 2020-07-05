using System.Security.Principal;


namespace GoodToCode.Framework.Security
{
    /// <summary>
    /// User Identity based on IPrincipal and IIdentity
    /// </summary>
    public class UserIdentity : IIdentity
    {
        /// <summary>
        ///  Authentication Type
        /// </summary>
        public string AuthenticationType { get; protected set; } = string.Empty;

        /// <summary>
        /// Is Authenticated
        /// </summary>
        public bool IsAuthenticated { get; protected set; } = false;

        /// <summary>
        /// User running process is IPrincipal.Name
        /// User logged in is IIdentity.Name
        /// </summary>
        public string Name { get; protected set; } = string.Empty;
    }
}
