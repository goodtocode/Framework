using GoodToCode.Framework.Activity;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Base used for all entity classes
    /// </summary>
    public interface IDataObject : IKey, ICreatedDate, IModifiedDate
    {
    }
}
