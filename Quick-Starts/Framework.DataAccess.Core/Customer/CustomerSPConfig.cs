using GoodToCode.Framework.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Framework.Customer
{
    /// <summary>
    /// Database-first entity, Code bound directly to View       
    /// </summary>
    public class CustomerSPConfig : StoredProcedureConfiguration<CustomerInfo>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity"></param>
        public CustomerSPConfig(CustomerInfo entity) : base(entity)
        {
        }

        /// <summary>
        /// Entity Create/Insert Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> CreateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoInsert",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Key", Entity.Key),
                new SqlParameter("@FirstName", Entity.FirstName),
                new SqlParameter("@MiddleName", Entity.MiddleName),
                new SqlParameter("@LastName", Entity.LastName),
                new SqlParameter("@BirthDate", Entity.BirthDate),
                new SqlParameter("@GenderId", Entity.GenderId),
                new SqlParameter("@CustomerTypeId", Entity.CustomerTypeId),
                new SqlParameter("@ActivityContextKey", Entity.ActivityContextKey)
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
                new SqlParameter("@Id", Entity.Id),
                new SqlParameter("@Key", Entity.Key),
                new SqlParameter("@FirstName", Entity.FirstName),
                new SqlParameter("@MiddleName", Entity.MiddleName),
                new SqlParameter("@LastName", Entity.LastName),
                new SqlParameter("@BirthDate", Entity.BirthDate),
                new SqlParameter("@GenderId", Entity.GenderId),
                new SqlParameter("@CustomerTypeId", Entity.CustomerTypeId),
                new SqlParameter("@ActivityContextKey", Entity.ActivityContextKey)
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
                new SqlParameter("@Id", Entity.Id),
                new SqlParameter("@Key", Entity.Key),
                new SqlParameter("@ActivityContextKey", Entity.ActivityContextKey)
            }
        };
    }
}
