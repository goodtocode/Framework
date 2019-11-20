using GoodToCode.Extensions;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Entity
{
    /// <summary>
    ///  ActiveRecord abstract class. Based on StoredProcedureEntity
    ///    Includes GetById(), GetByKey(), GetByWhere(), GetAll(), Save(), Delete()
    /// </summary>
    public abstract class ActiveRecordEntity<TEntity, TConfig> : EntityBase<TEntity>
        where TEntity : EntityBase<TEntity>, new()
        where TConfig : IEntityConfiguration<TEntity>, new()
    {
        /// <summary>
        /// Configuration for the C-UD operation stored procedures
        /// </summary>
        public TConfig SPConfiguration { get; set; } = new TConfig();

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
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetAll();
            return returnValue;
        }

        /// <summary>
        /// Gets by an expression for Where clause
        /// </summary>
        /// <param name="expression">Expression for where clause, typically to filter</param>
        /// <returns></returns>
        public static IEnumerable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> expression)
        {
            var reader = new EntityReader<TEntity>();
            var returnValue = reader.GetByWhere(expression);
            return returnValue;
        }

        /// <summary>
        /// Inserts and Updates to database
        /// </summary>
        /// <returns></returns>
        public async Task<TEntity> SaveAsync()
        {
            using (var writer = new EntityWriter<TEntity>(this.CastOrFill<TEntity>(), SPConfiguration))
            {
                return await writer.SaveAsync();
            }
        }

        /// <summary>
        /// Delete from the datastore
        /// </summary>
        public async Task<TEntity> DeleteAsync()
        {
            using (var writer = new EntityWriter<TEntity>(this.CastOrFill<TEntity>(), SPConfiguration))
            {
                return await writer.DeleteAsync();
            }
        }
    }
}
