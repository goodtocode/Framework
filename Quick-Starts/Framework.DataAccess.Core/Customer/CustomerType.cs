using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using System;
using System.Linq;

namespace Framework.Customer
{
    /// <summary>
    /// Tests attributes        
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode")]
    public class CustomerType : ValueInfo<CustomerType>
    {
        /// <summary>
        /// CustomerTypeId enumeration for static values
        /// </summary>
        public struct Types
        {
            /// <summary>
            /// Default/No Type
            /// </summary>
            public static Guid None { get; set; } = Defaults.Guid;

            /// <summary>
            /// Standard customer
            /// </summary>
            public static Guid Standard { get; set; } = new Guid("BF3797EE-06A5-47F2-9016-369BEB21D944");

            /// <summary>
            /// Premium active customer
            /// </summary>
            public static Guid Premium { get; set; } = new Guid("36B08B23-0C1D-4488-B557-69665FD666E1");

            /// <summary>
            /// Lifetime status
            /// </summary>
            public static Guid Lifetime { get; set; } = new Guid("51A84CE1-4846-4A71-971A-CB610EEB4848");
        }

        /// <summary>
        /// Customer Type friendly Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerType()
            : base()
        {
        }

        /// <summary>
        /// Gets all records that exactly equal passed name
        /// </summary>
        /// <param name="name">Value to search CustomerTypeName field </param>
        /// <returns>All records matching the passed name</returns>
        public static IQueryable<CustomerType> GetByName(string name)
        {
            var reader = new ValueReader<CustomerType>();
            return reader.GetAll().Where(x => x.Name == name);
        }
    }
}
