using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Session
{
    /// <summary>
    /// Context of user, device, application for all session flows
    /// </summary>
    public interface ISessionContext : IContext, IEntityKey
    {
    }
}
