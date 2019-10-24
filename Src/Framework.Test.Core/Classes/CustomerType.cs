using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests attributes        
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode")]
    public class CustomerType : ValueInfo<CustomerType>
    {

    }
}
