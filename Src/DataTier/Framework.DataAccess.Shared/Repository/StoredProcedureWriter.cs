//-----------------------------------------------------------------------
// <copyright file="StoredProcedureWriter.cs" company="GoodToCode">
//      Copyright (c) GoodToCode. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using GoodToCode.Extensions;
using GoodToCode.Extras.Data;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class StoredProcedureWriter<TEntity> :
        ICreateOperation<TEntity>, IUpdateOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : StoredProcedureEntity<TEntity>, new()
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

        /// <summary>
        /// 
        /// </summary>
        public ParameterBehaviors ParameterBehavior { get; set; } = ParameterBehaviors.Named;

        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public IEntityConfiguration<TEntity> DatabaseConfig { get; set; } = new EntityConfiguration<TEntity>();

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = Defaults.Boolean;
                using (var connection = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanInsert(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (entity.IsNew && DatabaseConfig.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity be updated in the database
        /// </summary>
        /// <param name="entity">Entity to be updated in the datastore</param>
        /// <returns>True if rules and setup allow for update, else false</returns>
        public bool CanUpdate(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (!entity.IsNew && DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
            return returnValue;
        }

        /// <summary>
        /// Can the entity deleted from the database
        /// </summary>
        /// <param name="entity">Entity to be deleted in the datastore</param>
        /// <returns>True if rules and setup allow for delete, else false</returns>
        public bool CanDelete(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (!entity.IsNew && (DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.AllAccess || DatabaseConfig.DataAccessBehavior == DataAccessBehaviors.NoUpdate))
                returnValue = true;
            return returnValue;
        }
    }
}
