
using GoodToCode.Framework.Data;
using System;
using System.Collections.Generic;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Customer Search Results
    /// </summary>    
    public class CustomerSearchDto : EntityDto<CustomerSearchDto>
    {
        private int maxResults = 25;

        /// <summary>
        /// FirstName of customers
        /// </summary>     
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// MiddleName of customer
        /// </summary>
        public string MiddleName { get; set; } = string.Empty;

        /// <summary>
        /// LastName of customer
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public DateTime BirthDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);

        /// <summary>
        /// Gender of customer
        /// </summary>
        public int GenderId { get; set; } = -1;

        /// <summary>
        /// Type of customer
        /// </summary>
        public Guid CustomerTypeKey { get; set; } = Guid.Empty;

        /// <summary>
        /// Search results
        /// </summary>
        public List<CustomerDto> Results { get; set; } = new List<CustomerDto>();

        /// <summary>
        /// Maximum number of results to return
        ///  Will not accept negative number, flips back to default (25)
        /// </summary>
        public int MaxResults
        {
            get => maxResults;
            set => maxResults = value > 0 ? value : maxResults;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public CustomerSearchDto()
                : base()
        {
        }
    }
}