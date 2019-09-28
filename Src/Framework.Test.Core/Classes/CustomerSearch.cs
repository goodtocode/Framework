using GoodToCode.Extensions;
using System;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Simulates a customer business object search class, for passing over Http and binding to screens
    /// </summary>
    public class CustomerSearch
    {
        public int Id { get; set; } = Defaults.Integer;
        public Guid Key { get; set; } = Defaults.Guid;
        public string FirstName { get; set; } = Defaults.String;
        public string MiddleName { get; set; } = Defaults.String;
        public string LastName { get; set; } = Defaults.String;
        public DateTime BirthDate { get; set; } = Defaults.Date;
        public int GenderId { get; set; } = Defaults.Integer;
        public Guid CustomerTypeKey { get; set; } = Defaults.Guid;
        public DateTime CreatedDate { get; set; } = Defaults.Date;
        public DateTime ModifiedDate { get; set; } = Defaults.Date;
        public int ActivityContextId { get; set; } = Defaults.Integer;
        public List<CustomerInfo> Results { get; set; } = new List<CustomerInfo>();
        public CustomerSearch()
                : base()
        {
        }
    }
}
