using GoodToCode.Extensions.Serialization;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Base used for all Value classes
    /// </summary>
    public interface IValue : ICreatedDate, IModifiedDate, IRecordState
    {
    }
}
