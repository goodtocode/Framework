//-----------------------------------------------------------------------
// <copyright file="ValueModel.cs" company="GoodToCode">
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
using GoodToCode.Extras.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace GoodToCode.Framework.Data
{
	/// <summary>
	/// ModelBase
	/// </summary>
	/// <remarks>ModelBase</remarks>
	public abstract class ValueModel<TValue> : IValue, ISerializable<TValue>, INotifyPropertyChanged where TValue : ValueModel<TValue>, new()
	{
        private Guid key = Defaults.Guid;
        private DateTime createdDate = Defaults.Date;
        private DateTime modifiedDate = Defaults.Date;

        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get => key; set => SetField(ref key, value); }

        /// <summary>
        /// Date record was created
        /// </summary>
        public virtual DateTime CreatedDate { get => createdDate; set => SetField(ref createdDate, value); }

        /// <summary>
        /// Date record was modified
        /// </summary>
        public virtual DateTime ModifiedDate { get => modifiedDate; set => SetField(ref modifiedDate, value); }

        /// <summary>
        /// Status of this record
        /// </summary>
        public virtual Guid State { get; set; } = RecordStates.Default;

        /// <summary>
        /// Is this a new object not yet committed to the database (Id == -1)
        /// </summary>
        public virtual bool IsNew => (this.Key == Defaults.Guid);

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueModel() : base() { }

        /// <summary>
        /// Property changed event handler for INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Property changed event for INotifyPropertyChanged
        /// </summary>
        /// <param name="propertyName">String name of property</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Fills this object with another object's data (of the same type)
        /// </summary>
        /// <param name="newItem"></param>
        /// <remarks></remarks>
        public bool Equals(TValue newItem)
        {
            bool returnValue = Defaults.Boolean;
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
        public string Serialize()
        {
            var serializer = new JsonSerializer<TValue>(this.CastSafe<TValue>());
            return serializer.Serialize();
        }

        /// <summary>
        /// De-serializes a string into this object
        /// </summary>
        /// <returns></returns>
        public TValue Deserialize(string data)
        {
            var serializer = new JsonSerializer<TValue>(data);
            return serializer.Deserialize();
        }

        /// <summary>
        /// Null-safe cast to the type TValue
        /// </summary>
        /// <returns>This object casted to type TValue</returns>
        public TValue ToEntity()
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

        /// <summary>
        /// Sets the property data as well as fired OnPropertyChanged() for INotifyPropertyChanged
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetField<TField>(ref TField field, TField value,
        [CallerMemberName] string propertyName = null)
        {
            var returnValue = Defaults.Boolean;
            if (EqualityComparer<TField>.Default.Equals(field, value) == false)
            {
                field = value;
                OnPropertyChanged(propertyName);
                returnValue = true;
            }                
            return returnValue;
        }
    }
}
