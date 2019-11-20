using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Test
{
    public class CustomerSPConfig : EntityConfiguration<CustomerInfo>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerSPConfig()
        {
        }

        /// <summary>
        /// Constructor 
        /// OBSOLETE
        /// </summary>
        /// <param name="entity"></param>
        public CustomerSPConfig(CustomerInfo entity) : base(entity) { }

        /// <summary>
        /// Entity Create/Insert Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> CreateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoInsert",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Key", EntityData.Key),
                new SqlParameter("@FirstName", EntityData.FirstName),
                new SqlParameter("@MiddleName", EntityData.MiddleName),
                new SqlParameter("@LastName", EntityData.LastName),
                new SqlParameter("@BirthDate", EntityData.BirthDate),
                new SqlParameter("@GenderId", EntityData.GenderId),
                new SqlParameter("@CustomerTypeId", EntityData.CustomerTypeId)
            }
        };

        /// <summary>
        /// Entity Update Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> UpdateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoUpdate",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id", EntityData.Id),
                new SqlParameter("@Key", EntityData.Key),
                new SqlParameter("@FirstName", EntityData.FirstName),
                new SqlParameter("@MiddleName", EntityData.MiddleName),
                new SqlParameter("@LastName", EntityData.LastName),
                new SqlParameter("@BirthDate", EntityData.BirthDate),
                new SqlParameter("@GenderId", EntityData.GenderId),
                new SqlParameter("@CustomerTypeId", EntityData.CustomerTypeId)
            }
        };

        /// <summary>
        /// Entity Delete Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> DeleteStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoDelete",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id", EntityData.Id),
                new SqlParameter("@Key", EntityData.Key)
            }
        };
    }
}
