using GoodToCode.Framework.Data;
using System;

namespace GoodToCode.Framework.Hosting.Server
{
    /// <summary>
    /// Crud API that is stored procedure driven
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class CrudApiDatabase<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Type representing the CrudApiController
        /// </summary>
        public Type CrudType { get; set; }

        /// <summary>
        /// Route associated with this type
        /// </summary>
        public IStoredProcedureConfiguration<TEntity> Configuration { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiDatabase(IStoredProcedureConfiguration<TEntity> databaseConfig)
        {
            CrudType = typeof(TEntity);
            Configuration = databaseConfig;
        }
    }
}