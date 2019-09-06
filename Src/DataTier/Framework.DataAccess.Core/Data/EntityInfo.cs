//-----------------------------------------------------------------------
// <copyright file="EntityInfo.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using GoodToCode.Extensions;
using GoodToCode.Extensions.Data;
using GoodToCode.Extensions.Serialization;
using GoodToCode.Framework.Text;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Entity Data Access Object base class
    /// Purpose is to read/write this object to a 
    ///  database or file
    /// Use GoodToCode.Framework.Repository classes to read/write to database
    ///   Id and Key can be set before saving
    ///   Auto-tracks inserts/updates/deletes via Activity.ActivityContext
    ///   Auto-validates before saving
    /// </summary>
    /// <remarks></remarks>
    public abstract partial class EntityInfo<TEntity> : IEntity, ISerializable<TEntity>, IValidatable<TEntity> where TEntity : class, IValidatable<TEntity>, new()
    {
        /// <summary>
        /// Id of record
        ///  Can set Id before saving, and will be preserved
        ///  only if using GoodToCode.Framework.Repository for CRUD
        /// </summary>
        public virtual int Id { get; set; } = Defaults.Integer;

        /// <summary>
        /// Key of record
        ///  Can set Key before saving, and will be preserved
        ///  only if using GoodToCode.Framework.Repository for CRUD
        /// </summary>
        public virtual Guid Key { get; set; } = Defaults.Guid;

        /// <summary>
        /// Activity history that created this record
        /// </summary>
        public virtual Guid ActivityContextKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = Defaults.Date;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = Defaults.Date;

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
                var returnValue = Defaults.Boolean;
                switch (new TEntity().GetAttributeValue<RecordIdentity, Fields>(Fields.IdOrKey))
                {
                    case Fields.Id:
                        returnValue = Id == Defaults.Integer;
                        break;
                    case Fields.Key:
                        returnValue = Key == Defaults.Guid;
                        break;
                    case Fields.IdOrKey:
                        returnValue = Id == Defaults.Integer || Key == Defaults.Guid;
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
        public EntityInfo() : base() { }

        /// <summary>
        /// Fills this object with another object's data (of the same type)
        /// </summary>
        /// <param name="newItem"></param>
        /// <remarks></remarks>
        public bool Equals(TEntity newItem)
        {
            var returnValue = Defaults.Boolean;
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
