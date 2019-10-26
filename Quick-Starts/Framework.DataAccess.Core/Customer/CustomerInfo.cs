using GoodToCode.Extensions;
using GoodToCode.Extensions.Text.Cleansing;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Customer
{
    /// <summary>
    /// Database-first entity, Code bound directly to View
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode"), TableName("CustomerInfo")]
    public class CustomerInfo : EntityInfo<CustomerInfo>
    {
        /// <summary>
        /// Business and Validation rules
        /// </summary>
        public override IList<IValidationRule<CustomerInfo>> Rules()
        {
            return new List<IValidationRule<CustomerInfo>>()
            {
                new ValidationRule<CustomerInfo>(x => x.FirstName.Length > 0, "First Name is required."),
                new ValidationRule<CustomerInfo>(x => x.LastName.Length > 0, "Last Name is required.")
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
            /// Default. Not set
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
        public CustomerInfo() : base() { }
    }
}
