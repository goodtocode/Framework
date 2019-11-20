namespace GoodToCode.Framework.Entity
{
    /// <summary>
    /// Configures stored procedure for specific parameter behavior
    /// Named: @Param1 is used to match parameter with entity data. 
    ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
    /// Ordinal: @Param1, @Param2 are assigned in ordinal position
    ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
    /// </summary>
    public enum ParameterBehaviors
    {
        /// <summary>
        /// Named parameter matching behavior
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.ToString());
        /// </summary>
        Named = 0x0,

        /// <summary>
        /// Ordinal position parameter behavior
        ///   - Uses: Database.ExecuteSqlCommand(entity.CreateStoredProcedure.SqlPrefix, entity.CreateStoredProcedure.Parameters.ToArray());
        /// </summary>
        Ordinal = 0x2
    }
}
