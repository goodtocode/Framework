﻿using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Test
{
    public class CustomerSPConfig : EntityWriterConfiguration<CustomerInfo>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerSPConfig(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="entity"></param>
        public CustomerSPConfig(string connectionString, CustomerInfo entity) : base(connectionString, entity) { }

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
                new SqlParameter("@CustomerTypeId", Entity.CustomerTypeId)
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
                new SqlParameter("@CustomerTypeId", Entity.CustomerTypeId)
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
                new SqlParameter("@Key", Entity.Key)
            }
        };
    }
}
