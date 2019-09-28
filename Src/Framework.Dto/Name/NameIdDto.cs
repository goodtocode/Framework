using System;
using GoodToCode.Extensions;
using GoodToCode.Framework.Data;

namespace GoodToCode.Framework.Name
{
	/// <summary>
	/// Common object across models and business entity
	/// </summary>
	/// <remarks></remarks>
	public class NameIdDto : EntityDto<NameIdDto>, INameId
	{
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = Defaults.String;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NameIdDto() : base()
		{
		}
	}
}
