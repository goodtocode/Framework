using GoodToCode.Framework.Text;
using System.Collections.Generic;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// Supports self-validation, especially when data is to be persisted to the database.
    /// </summary>
    public interface IValidatable<TEntity>
    {
        /// <summary>
        /// Business Rules to validate
        /// </summary>
        IList<IValidationRule<TEntity>> Rules();

        /// <summary>
        /// Rules that failed validation
        /// </summary>
        IList<ITextMessage> FailedRules { get; }
    }
}
