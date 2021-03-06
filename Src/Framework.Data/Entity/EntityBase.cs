using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Text;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// Entity Data Access Object base class
    /// Purpose is to read/write this object to a 
    ///  database or file
    /// Use GoodToCode.Framework.Repository classes to read/write to database
    ///   Id and Key can be set before saving
    ///   Auto-validates before saving
    /// </summary>
    /// <remarks></remarks>
    public abstract class EntityBase<TEntity> : IEntity, ISerializable<TEntity>, IValidatable<TEntity> where TEntity : class, IValidatable<TEntity>, new()
    {
        /// <summary>
        /// Id of record
        ///  Can set Id before saving, and will be preserved
        ///  only if using GoodToCode.Framework.Repository for CRUD
        /// </summary>
        public virtual int Id { get; set; } = -1;

        /// <summary>
        /// Key of record
        ///  Can set Key before saving, and will be preserved
        ///  only if using GoodToCode.Framework.Repository for CRUD
        /// </summary>
        public virtual Guid Key { get; set; } = Guid.Empty;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = new DateTime(1900, 01, 01, 00, 00, 00, 000, DateTimeKind.Utc);

        /// <summary>
        /// Status of this record
        /// </summary>
        public virtual Guid State { get; set; } = RecordStates.Default;

        /// <summary>
        /// Is this a new object not yet committed to the database (Id == -1)
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                var returnValue = false;
                switch (new TEntity().GetAttributeValue<RecordIdentity, Fields>(Fields.IdOrKey))
                {
                    case Fields.Id:
                        returnValue = Id == -1;
                        break;
                    case Fields.Key:
                        returnValue = Key == Guid.Empty;
                        break;
                    case Fields.IdOrKey:
                        returnValue = Id == -1 || Key == Guid.Empty;
                        break;
                    case Fields.None:
                    default:
                        returnValue = true;
                        break;
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Rules that failed validation
        /// </summary>
        public IList<ITextMessage> FailedRules { get; protected set; } = new List<ITextMessage>();

        /// <summary>
        /// Rules used by the validator for Data Validation and Business Validation
        /// </summary>
        public abstract IList<IValidationRule<TEntity>> Rules();

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityBase() : base() { }

        /// <summary>
        /// Fills this object with another object's data (of the same type)
        /// </summary>
        /// <param name="newItem"></param>
        /// <remarks></remarks>
        public bool Equals(TEntity newItem)
        {
            var returnValue = false;
            var newObjectType = newItem.GetType();

            returnValue = true;
            foreach (var newObjectProperty in newObjectType.GetRuntimeProperties())
            {
                var currentProperty = typeof(TEntity).GetRuntimeProperty(newObjectProperty.Name);
                if ((currentProperty != null && currentProperty.CanWrite)
                    && (object.Equals(currentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false))
                {
                    returnValue = false;
                    break;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Validates TEntity.Rules() collection
        /// Returns failed rules collection
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITextMessage> Validate()
        {
            var validator = new EntityValidator<TEntity>(this.CastSafe<TEntity>());
            FailedRules = validator.Validate();
            return FailedRules;
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return !Validate().Any();
        }        

        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            var serializer = new JsonSerializer<TEntity>(this.CastSafe<TEntity>());
            return serializer.Serialize();
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public TEntity Deserialize(string data)
        {
            var serializer = new JsonSerializer<TEntity>(data);
            return serializer.Deserialize();
        }

        /// <summary>
        /// Null-safe cast to the type TEntity
        /// </summary>
        /// <returns>This object casted to type TEntity</returns>
        public TEntity ToEntity()
        {
            return this.CastOrFill<TEntity>();
        }

        /// <summary>
        /// Start with Id as string representation
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
