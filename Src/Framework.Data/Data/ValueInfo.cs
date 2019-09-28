using GoodToCode.Extensions;
using GoodToCode.Extensions.Serialization;
using System;
using System.Reflection;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// ReadValueBase
    /// </summary>
    /// <remarks>ReadValueBase</remarks>
    public abstract partial class ValueInfo<TValue> : IValue, ISerializable<TValue> where TValue : class, IValue, ISerializable<TValue>, new()
    {        
        /// <summary>
        /// Guid of record
        /// </summary>
        public virtual Guid Key { get; set; } = Defaults.Guid;

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
        /// Forces initialization of EF-generated properties (PropertyValue = Defaults.{Type})
        /// </summary>
        public ValueInfo() : base() { }

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
