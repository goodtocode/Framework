using System;

using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Name
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	public class NameDescriptionDto : EntityDto<NameDescriptionDto>, INameDescription
	{
        /// <summary>
        /// Name
        /// </summary>
		public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
		public string Description { get; set; } = string.Empty;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks></remarks>
		public NameDescriptionDto() : base()
		{
		}
	}
}
