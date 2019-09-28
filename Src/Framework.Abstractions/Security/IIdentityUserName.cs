namespace GoodToCode.Framework.Security
{
    /// <summary>
    /// User of any system
    /// </summary>
    public interface IIdentityUserName
    {
        /// <summary>
        /// User name of the person logged in, not the principal user name of the process doing the work
        /// </summary>
        string IdentityUserName { get; }
    }
}
