using GoodToCode.Framework.Device;
using GoodToCode.Framework.Security;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Activity record tracking the a transactional process, typically querying or committing of data.
    /// </summary>
    public interface IContext : IIdentityUserName, IDeviceUuid, IApplicationUuid
    {

    }
}
