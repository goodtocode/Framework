using GoodToCode.Framework.Data;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Tests attributes        
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode")]
    public partial class CustomerType : ValueInfo<CustomerType>
    {

    }
}
