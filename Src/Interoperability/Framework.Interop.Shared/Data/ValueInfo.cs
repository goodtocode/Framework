//-----------------------------------------------------------------------
// <copyright file="ValueInfo.cs" company="Genesys Source">
//      Copyright (c) Genesys Source. All rights reserved.
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
using Genesys.Extensions;
using Genesys.Extras.Serialization;
using System;
using System.Reflection;
using Genesys.Framework.Validation;
using System.Collections.Generic;

namespace Genesys.Framework.Data
{
    /// <summary>
    /// ReadValueBase
    /// </summary>
    /// <remarks>ReadValueBase</remarks>
    public abstract partial class ValueInfo<TValue> : IValue<TValue> where TValue : class, IValue<TValue>, new()
    {
        /// <summary>
        /// Id of record
        /// </summary>
        public virtual int Id { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get; set; } = TypeExtension.DefaultGuid;

        /// <summary>
        /// Workflow activity that created this record
        /// </summary>
        public virtual int ActivityContextId { get; set; } = TypeExtension.DefaultInteger;

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// Status of this record
        /// </summary>
        public virtual RecordStates Status { get; set; } = RecordStates.Default;

        /// <summary>
        /// Is this a new object not yet committed to the database (Id == -1)
        /// </summary>
        public virtual bool IsNew
        {
            get
            {
                return (this.Id == TypeExtension.DefaultInteger);
            }
        }

        /// <summary>
        /// Serialize and Deserialize this class
        /// </summary>
        private ISerializer<TValue> serializer = new JsonSerializer<TValue>();

        /// <summary>
        /// Validator providing Validate(), CanSave()
        /// </summary>
        private IValidator<TValue> validator = new Validator<TValue>();

        /// <summary>
        /// Forces initialization of EF-generated properties (PropertyValue = TypeExtension.Default{Type})
        /// </summary>
        public ValueInfo() : base() { this.Initialize<ValueInfo<TValue>>(); }

        /// <summary>
        /// Fills this object with another object's data (of the same type)
        /// </summary>
        /// <param name="newItem"></param>
        /// <remarks></remarks>
        public bool Equals(TValue newItem)
        {
            bool returnValue = TypeExtension.DefaultBoolean;
            Type newObjectType = newItem.GetType();

            // Start True
            returnValue = true;
            // Loop through all new item's properties
            foreach (var newObjectProperty in newObjectType.GetRuntimeProperties())
            {
                // Copy the data using reflection
                PropertyInfo currentProperty = typeof(TValue).GetRuntimeProperty(newObjectProperty.Name);
                if (currentProperty != null && currentProperty.CanWrite)
                {
                    // Check for equivalence
                    if (object.Equals(currentProperty.GetValue(this, null), newObjectProperty.GetValue(newItem, null)) == false)
                    {
                        returnValue = false;
                        break;
                    }
                }
            }

            // Return data
            return returnValue;
        }

        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IValidationRule<TValue>> Validate() => validator.Validate(this.CastSafe<TValue>());

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public bool IsValid() => validator.IsValid(this.CastSafe<TValue>());

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public bool CanSave() => validator.CanSave(this.CastSafe<TValue>());

        /// <summary>
        /// Serializes this object into a Json string
        /// </summary>
        /// <returns></returns>
        public string Serialize() => serializer.Serialize(this.ToValue());

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public TValue Deserialize(string data)
        {
            return serializer.Deserialize(data);
        }

        /// <summary>
        /// Null-safe cast to the type TValue
        /// </summary>
        /// <returns>This object casted to type TValue</returns>
        public TValue ToValue()
        {
            return this.CastOrFill<TValue>();
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
