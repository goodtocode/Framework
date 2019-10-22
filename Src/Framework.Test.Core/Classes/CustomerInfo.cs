using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Database-first entity, Code bound directly to View       
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode"),
        TableName("CustomerInfo")]
    public partial class CustomerInfo : StoredProcedureEntity<CustomerInfo>
    {
        /// <summary>
        /// Entity Create/Insert Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> CreateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoInsert",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Key", Key),
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@MiddleName", MiddleName),
                new SqlParameter("@LastName", LastName),
                new SqlParameter("@BirthDate", BirthDate),
                new SqlParameter("@GenderId", GenderId),
                new SqlParameter("@CustomerTypeId", CustomerTypeId),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
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
                new SqlParameter("@Id", Id),
                new SqlParameter("@Key", Key),
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@MiddleName", MiddleName),
                new SqlParameter("@LastName", LastName),
                new SqlParameter("@BirthDate", BirthDate),
                new SqlParameter("@GenderId", GenderId),
                new SqlParameter("@CustomerTypeId", CustomerTypeId),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
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
                new SqlParameter("@Id", Id),
                new SqlParameter("@Key", Key),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
            }
        };

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
