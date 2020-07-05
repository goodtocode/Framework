
using System;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Simulates a customer business object search class, for passing over Http and binding to screens
    /// </summary>
    public class CustomerSearch
    {
        public int Id { get; set; } = -1;
        public Guid Key { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);
        public int GenderId { get; set; } = -1;
        public Guid CustomerTypeKey { get; set; } = Guid.Empty;
        public DateTime CreatedDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);
        public DateTime ModifiedDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);
        public List<CustomerInfo> Results { get; set; } = new List<CustomerInfo>();
        public CustomerSearch()
                : base()
        {
        }
    }
}
