
using GoodToCode.Framework.Text;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Framework.Validation
{
    /// <summary>
    /// Self-validating rule based on Lambda expression
    /// </summary>
    /// <typeparam name="TEntity">Entity to validate</typeparam>
    public class EntityValidator<TEntity> : IEntityValidator<TEntity> where TEntity : IValidatable<TEntity>, new()
    {
        private TEntity entity = new TEntity();
        private bool hasValidated = false;

        /// <summary>
        /// Business rules to run
        /// </summary>
        public IList<IValidationRule<TEntity>> Rules() { return entity.Rules() ?? new List<IValidationRule<TEntity>>(); } 

        /// <summary>
        /// Rules that failed validation
        /// </summary>
        public IList<ITextMessage> FailedRules { get; private set; } = new List<ITextMessage>();

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityValidator(TEntity entityToValidate) : base() { entity = entityToValidate; }
        
        /// <summary>
        /// Runs all business rules
        /// </summary>        
        public IList<ITextMessage> Validate()
        {
            foreach (var Item in Rules())
            {
                Item.Validate(entity);
                if(!Item.IsValid) FailedRules.Add(Item.Result);
            }
            hasValidated = true;
            return FailedRules;
        }
        
        /// <summary>
        /// Determines if all items are valid
        /// </summary>        
        public bool IsValid()
        {
            if(!hasValidated) Validate();
            return (!FailedRules.Any());
        }
    }
}
