using System;
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Test
{
    /// <summary>
    /// Customer screen model for binding and transport
    /// </summary>
    /// <remarks></remarks>
    public class CustomerDto : EntityDto<CustomerDto>, IFormattable
    {
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
        /// Gnder of customer
        /// </summary>
        public int GenderId { get; set; } = Defaults.Integer;

        /// <summary>
        /// Type of customer
        /// </summary>
        public Guid CustomerTypeKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public CustomerDto()
                : base()
        {
        }

        /// <summary>
        /// Supports fml (First Middle Last), lfm (Last, First Middle)
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (formatProvider != null)
            {
                if (formatProvider.GetFormat(this.GetType()) is ICustomFormatter fmt) { return fmt.Format(format, this, formatProvider); }
            }
            switch (format)
            {
                case "lfm": return String.Format("{0}, {1} {2}", this.LastName, this.FirstName, this.MiddleName);
                case "lfMI": return String.Format("{0}, {1} {2}.", this.LastName, this.FirstName, this.MiddleName.SubstringSafe(0, 1));
                case "fMIl": return String.Format("{0} {1}. {2}", this.FirstName, this.MiddleName.SubstringSafe(0, 1), this.LastName);
                case "fl": return String.Format("{0} {1}", this.FirstName, this.LastName);
                case "fml":
                case "G":
                default: return String.Format("{0} {1} {2}", this.FirstName, this.MiddleName, this.LastName);
            }
        }
    }
}
