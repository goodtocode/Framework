namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  StoredProcedureEntity - C-UD operations of an entity against stored procedures
    ///   Read remains EntityReader centric
    /// </summary>
    public abstract partial class StoredProcedureEntity<TEntity> : EntityInfo<TEntity> where TEntity : StoredProcedureEntity<TEntity>, new()
    {
        /// <summary>
        /// Stored procedure that creates the entity
        /// </summary>
        public abstract StoredProcedure<TEntity> CreateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that updates the entity
        /// </summary>
        public abstract StoredProcedure<TEntity> UpdateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that deletes the entity
        /// </summary>
        public abstract StoredProcedure<TEntity> DeleteStoredProcedure { get; }
    }
}
