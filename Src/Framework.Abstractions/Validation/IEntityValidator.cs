using GoodToCode.Framework.Text;
using System.Collections.Generic;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// Supports self-validation, especially when data is to be persisted to the database.
    /// </summary>
    public interface IEntityValidator<TEntity> : IValidatable<TEntity>
    {
        /// <summary>
        /// Validate all rules
        /// </summary>        
        IList<ITextMessage> Validate();

        /// <summary>
        /// Validate all rules and return valid true/false
        /// </summary>        
        bool IsValid();
    }
}
