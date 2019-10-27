using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Entity;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Test
{
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

    /// <summary>
    /// Database-first entity, Code bound directly to View       
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode"),
        TableName("CustomerInfo")]
    public class CustomerInfo : EntityInfo<CustomerInfo>
    {        
        /// <summary>
        /// ValidationRules and BusinessRules that ensure no dirty data is committed
        /// </summary>
        public override IList<IValidationRule<CustomerInfo>> Rules()
        {
            return new List<IValidationRule<CustomerInfo>>()
            {
                new ValidationRule<CustomerInfo>(x => x.FirstName.Length > 0, "First Name is required.")
            };
        }

        /// <summary>
        /// FirstName of customers
        /// </summary>
        public string FirstName { get; set; } = Defaults.String;

        /// <summary>
        /// MiddleName of customer
        /// </summary>
        public string MiddleName { get; set; } = Defaults.String;

        /// <summary>
        /// LastName of customer
        /// </summary>
        public string LastName { get; set; } = Defaults.String;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public DateTime BirthDate { get; set; } = Defaults.Date;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public int GenderId { get; set; } = Genders.NotSet.Key;

        /// <summary>
        /// Type of customer
        /// </summary>
        public int CustomerTypeId { get; set; } = Defaults.Integer;

        /// <summary>
        /// ISO 5218 Standard for Gender values
        /// </summary>
        public struct Genders
        {
            /// <summary>
            /// Defaults. Not set
            /// </summary>
            public static KeyValuePair<int, string> NotSet { get; } = new KeyValuePair<int, string>(-1, "Not Set");

            /// <summary>
            /// Unknown gender
            /// </summary>
            public static KeyValuePair<int, string> NotKnown { get; } = new KeyValuePair<int, string>(0, "Not Known");

            /// <summary>
            /// Male gender
            /// </summary>
            public static KeyValuePair<int, string> Male { get; } = new KeyValuePair<int, string>(1, "Male");

            /// <summary>
            /// Femal Gender
            /// </summary>
            public static KeyValuePair<int, string> Female { get; } = new KeyValuePair<int, string>(2, "Female");

            /// <summary>
            /// Not applicable or do not want to specify
            /// </summary>
            public static KeyValuePair<int, string> NotApplicable { get; } = new KeyValuePair<int, string>(9, "Not Applicable");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerInfo() : base()
        {
        }
    }
}
