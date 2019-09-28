using GoodToCode.Framework.Text;
using System;

namespace GoodToCode.Framework.Validation
{   
    /// <summary>
    /// Validation Rule contract
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IValidationRule<TEntity>
    {
        /// <summary>
        /// Rule criteria that determines pass/fail
        /// </summary>
        Predicate<TEntity> Rule { get; }

        /// <summary>
        /// Is this rule valid
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Validate this entity
        /// </summary>
        /// <param name="entity"></param>
        
        bool Validate(TEntity entity);

        /// <summary>
        /// Type of rule, drives database behavior
        /// </summary>
        Guid ValidationRuleTypeKey { get; }

        /// <summary>
        /// Result message
        /// </summary>
        ITextMessage Result { get; }
    }    
}
