//using GoodToCode.Framework.Entity;

//namespace GoodToCode.Framework.Data
//{
//    /// <summary>
//    ///  StoredProcedureEntity - C-UD operations of an entity against stored procedures
//    ///   Read remains EntityReader centric
//    /// </summary>
//    public class StoredProcedureConfiguration<TEntity> : EntityConfiguration<TEntity>, IStoredProcedureConfiguration<TEntity> where TEntity : EntityBase<TEntity>, new()
//    {
//        /// <summary>
//        /// Entity to be applied to the stored procedure parameters
//        /// </summary>
//        public TEntity Entity { get; set; } = new TEntity();

//        /// <summary>
//        /// Constructor 
//        /// </summary>
//        public StoredProcedureConfiguration()
//        {
//        }

//        /// <summary>
//        /// Constructor 
//        /// </summary>
//        /// <param name="entity"></param>
//        public StoredProcedureConfiguration(TEntity entity)
//        {
//            Entity = entity;
//        }
//    }
//}
