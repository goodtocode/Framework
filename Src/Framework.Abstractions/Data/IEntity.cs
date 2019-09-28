using GoodToCode.Framework.Activity;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Base used for all entity classes
    /// </summary>
    public interface IEntity : IId, IKey, IActivityContextKey, ICreatedDate, IModifiedDate
    {
        /// <summary>
        /// Is a new object, and most likely not yet committed to the database
        /// </summary>
        bool IsNew { get; }
    }
}
