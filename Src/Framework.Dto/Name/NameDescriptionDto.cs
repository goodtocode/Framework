using System;
using GoodToCode.Extensions;
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
		public string Name { get; set; } = Defaults.String;

        /// <summary>
        /// Description
        /// </summary>
		public string Description { get; set; } = Defaults.String;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <remarks></remarks>
		public NameDescriptionDto() : base()
		{
		}
	}
}
