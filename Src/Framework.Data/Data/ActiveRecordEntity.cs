using GoodToCode.Extensions;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  ActiveRecord abstract class. Based on StoredProcedureEntity
    ///    Includes GetById(), GetByKey(), GetByWhere(), GetAll(), Save(), Delete()
    /// </summary>
    public abstract class ActiveRecordEntity<TEntity> : StoredProcedureConfiguration<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Activity record for this entities behavior
        /// </summary>
        public Guid ActivityContextKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Gets single record by Id
        /// </summary>
        /// <param name="id">Integer Id that is the primary key or a unique identity</param>
        /// <returns></returns>
        public static TEntity GetById(int id)
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var returnValue = reader.GetById(id);
                return returnValue;
            }
        }

        /// <summary>
        /// Gets single record by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TEntity GetByKey(Guid key)
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var returnValue = reader.GetByKey(key);
                return returnValue;
            }
        }

        /// <summary>
        /// Generic read functionality
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetAll()
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var returnValue = reader.GetAll();
                return returnValue;
            }
        }

        /// <summary>
        /// Gets by an expression for Where clause
        /// </summary>
        /// <param name="expression">Expression for where clause, typically to filter</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> expression)
        {
            using (var reader = new EntityReader<TEntity>())
            {
                var returnValue = reader.GetByWhere(expression);
                return returnValue;
            }
        }

        /// <summary>
        /// Inserts and Updates to database
        /// </summary>
        /// <returns></returns>
        public TEntity Save()
        {
            using (var writer = new StoredProcedureWriter<TEntity>(this))
            {
                return writer.Save(this.CastOrFill<TEntity>());
            }
        }

        /// <summary>
        /// Inserts and Updates to database
        /// </summary>
        /// <param name="activity">Activity responsible for tracking this process</param>        
        public TEntity Save(IActivityContext activity)
        {
            ActivityContextKey = activity.ActivityContextKey;
            return Save();
        }

        /// <summary>
        /// Delete from the datastore
        /// </summary>
        public TEntity Delete()
        {
            using (var writer = new StoredProcedureWriter<TEntity>(this))
            {
                return writer.Delete(this.CastOrFill<TEntity>());
            }
        }

        /// <summary>
        /// Delete from the datastore
        /// </summary>
        public TEntity Delete(IActivityContext activity)
        {
            ActivityContextKey = activity.ActivityContextKey;
            return Delete();
        }
    }
}
