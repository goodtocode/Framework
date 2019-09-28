using System;
using System.Security.Principal;
using GoodToCode.Framework.Session;

namespace GoodToCode.Framework.Security
{
    /// <summary>
    /// User of any system
    /// </summary>
    public interface IUserPrincipal : IPrincipal, ISessionContext
    {
    }
}
