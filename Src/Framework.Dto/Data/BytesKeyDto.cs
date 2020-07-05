using System;


namespace GoodToCode.Framework.Data
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	public class BytesKeyDto : EntityDto<BytesKeyDto>, IBytesKey
	{
        /// <summary>
        /// Bytes of BLOB
        /// </summary>
        public byte[] Bytes { get; set; } = new byte[] { 0, 0, 0, 1 };
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks></remarks>
		public BytesKeyDto() : base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public BytesKeyDto(byte[] bytes)
            : this()
        {
            Bytes = bytes;
        }		
	}
}
