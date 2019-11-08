using GoodToCode.Framework.Data;
using GoodToCode.Framework.Value;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests attributes        
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode")]
    public class CustomerType : ValueBase<CustomerType>
    {

    }
}
