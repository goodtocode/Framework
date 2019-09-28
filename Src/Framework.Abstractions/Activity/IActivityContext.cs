using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Activity
{
    /// <summary>
    /// Activity record tracking the a transactional process, typically querying or committing of data.
    /// </summary>
    public interface IActivityContext : IContext, IActivityContextId, IActivityContextKey, ICreatedDate
    {
        /// <summary>
        /// IP4 Address of the process executing this activity
        /// </summary>
        string PrincipalIP4Address { get; set; }

        /// <summary>
        /// Runtime context of this activity
        /// </summary>
        string ExecutingContext { get; set; }
    }
}
