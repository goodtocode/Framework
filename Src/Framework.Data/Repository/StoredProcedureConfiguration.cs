namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  StoredProcedureEntity - C-UD operations of an entity against stored procedures
    ///   Read remains EntityReader centric
    /// </summary>
    public class StoredProcedureConfiguration<TEntity> : IStoredProcedureConfiguration<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>
        public TEntity Entity { get; set; } = new TEntity();

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="entity"></param>
        public StoredProcedureConfiguration(TEntity entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Stored procedure that creates the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> CreateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that updates the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> UpdateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that deletes the entity
        /// </summary>
        public virtual StoredProcedure<TEntity> DeleteStoredProcedure { get; }
    }
}
