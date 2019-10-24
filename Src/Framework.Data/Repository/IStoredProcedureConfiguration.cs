namespace GoodToCode.Framework.Data
{
    /// <summary>
    ///  StoredProcedureEntity - C-UD operations of an entity against stored procedures
    ///   Read remains EntityReader centric
    /// </summary>
    public interface IStoredProcedureConfiguration<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Entity to be applied to the stored procedure parameters
        /// </summary>
        TEntity Entity { get; }

        /// <summary>
        /// Stored procedure that creates the entity
        /// </summary>
        StoredProcedure<TEntity> CreateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that updates the entity
        /// </summary>
        StoredProcedure<TEntity> UpdateStoredProcedure { get; }

        /// <summary>
        /// Stored procedure that deletes the entity
        /// </summary>
        StoredProcedure<TEntity> DeleteStoredProcedure { get; }
    }
}
