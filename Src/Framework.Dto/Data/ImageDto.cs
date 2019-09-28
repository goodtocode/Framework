using System;
using GoodToCode.Extensions;
using GoodToCode.Framework.Name;

namespace GoodToCode.Framework.Data
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	public class ImageDto : EntityDto<ImageDto>, IBytesKey, IName
    {
        /// <summary>
        /// Name of this image
        /// </summary>
        public string Name { get; set; } = Defaults.String;

        /// <summary>
        /// Bytes of BLOB image
        /// </summary>
        public byte[] Bytes { get; set; } = Defaults.Bytes;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ImageDto() : base()
		{
		}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public ImageDto(byte[] bytes)
            : this()
        {
            Bytes = bytes;
        }
		
	}
}
