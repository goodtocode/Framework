//-----------------------------------------------------------------------
// <copyright file="EntityWriter.cs" company="GoodToCode">
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
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using System.Data.SqlClient;

namespace GoodToCode.Framework.Repository
{
    /// <summary>
    /// EF DbContext for read-only GetBy* operations
    /// </summary>
    public partial class EntityWriter<TEntity> : ISaveOperation<TEntity>, IDeleteOperation<TEntity> where TEntity : EntityInfo<TEntity>, new()
    {
        /// <summary>
        /// Configuration class for dbContext options
        /// </summary>
        public virtual IEntityConfiguration<TEntity> ConfigOptions { get; set; } = new EntityConfiguration<TEntity>(prop => prop.Id);

        /// <summary>
        /// Can connect to database?
        /// </summary>
        public bool CanConnect
        {
            get
            {
                var returnValue = Defaults.Boolean;
                using (var connection = new SqlConnection(ConfigOptions.ConnectionString))
                {
                    returnValue = connection.CanOpen();
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Inserts this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Create(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
        }

        /// <summary>
        /// Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Update(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Delete(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Delete(entity);
        }

        /// <summary>
        /// Inserts or Updates this object with Workflow-based tracking.
        /// </summary>  
        /// <param name="entity">TEntity entity to commit to data storage</param>
        /// <param name="activity">Activity record owning this process</param>
        public virtual TEntity Save(TEntity entity, IActivityContext activity)
        {
            entity.ActivityContextKey = activity.ActivityContextKey;
            return Save(entity);
        }

        /// <summary>
        /// Can the entity insert to the database
        /// </summary>
        /// <param name="entity">Entity to be saved to datastore</param>
        /// <returns>True if rules and setup allow for insert, else false</returns>
        public bool CanInsert(TEntity entity)
        {
            var returnValue = Defaults.Boolean;
            if (entity.IsNew && ConfigOptions.DataAccessBehavior != DataAccessBehaviors.SelectOnly)
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
            if (!entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
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
            if (!entity.IsNew && ConfigOptions.DataAccessBehavior == DataAccessBehaviors.AllAccess)
                returnValue = true;
            return returnValue;
        }
    }
}
