using GoodToCode.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  ActiveRecord abstract class. Based on StoredProcedureValue
    ///    Includes GetById(), GetByKey(), GetByWhere(), GetAll(), Save(), Delete()
    /// </summary>
    public abstract class ActiveRecordValue<TValue> : ValueInfo<TValue> where TValue : ValueInfo<TValue>, new()
    {
        /// <summary>
        /// Gets single record by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue GetByKey(Guid key)
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetByKey(key);
            return returnValue;
        }

        /// <summary>
        /// Generic read functionality
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TValue> GetAll()
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetAll();
            return returnValue;
        }

        /// <summary>
        /// Gets by an expression for Where clause
        /// </summary>
        /// <param name="expression">Expression for where clause, typically to filter</param>
        /// <returns></returns>
        public static IEnumerable<TValue> GetByWhere(Expression<Func<TValue, bool>> expression)
        {
            var reader = new ValueReader<TValue>();
            var returnValue = reader.GetByWhere(expression);
            return returnValue;
        }
    }
}
