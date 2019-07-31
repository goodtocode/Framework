//-----------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="GoodToCode">
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
using GoodToCode.Extras.Text.Cleansing;
using GoodToCode.Framework.Activity;
using GoodToCode.Framework.Data;
using GoodToCode.Framework.Operation;
using GoodToCode.Framework.Repository;
using GoodToCode.Framework.Validation;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Customer
{
    /// <summary>
    /// Database-first entity, Code bound directly to View       
    /// </summary>
    [ConnectionStringName("DefaultConnection"), DatabaseSchemaName("CustomerCode"), TableName("CustomerInfo")]
    public partial class CustomerInfo : StoredProcedureEntity<CustomerInfo>, IReadOperation<CustomerInfo>
    {
        /// <summary>
        /// Entity Create/Insert Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> CreateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoInsert",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Key", Key),
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@MiddleName", MiddleName),
                new SqlParameter("@LastName", LastName),
                new SqlParameter("@BirthDate", BirthDate),
                new SqlParameter("@GenderId", GenderId),
                new SqlParameter("@CustomerTypeId", CustomerTypeId),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
            }
        };

        /// <summary>
        /// Entity Update Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> UpdateStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoUpdate",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id", Id),
                new SqlParameter("@Key", Key),
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@MiddleName", MiddleName),
                new SqlParameter("@LastName", LastName),
                new SqlParameter("@BirthDate", BirthDate),
                new SqlParameter("@GenderId", GenderId),
                new SqlParameter("@CustomerTypeId", CustomerTypeId),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
            }
        };

        /// <summary>
        /// Entity Delete Stored Procedure
        /// </summary>
        public override StoredProcedure<CustomerInfo> DeleteStoredProcedure
        => new StoredProcedure<CustomerInfo>()
        {
            StoredProcedureName = "CustomerInfoDelete",
            Parameters = new List<SqlParameter>()
            {
                new SqlParameter("@Id", Id),
                new SqlParameter("@Key", Key),
                new SqlParameter("@ActivityContextKey", ActivityContextKey)
            }
        };

        /// <summary>
        /// Business and Validation rules
        /// </summary>
        public override IList<IValidationRule<CustomerInfo>> Rules()
        {
            return new List<IValidationRule<CustomerInfo>>()
            {
                new ValidationRule<CustomerInfo>(x => x.FirstName.Length > 0, "First Name is required."),
                new ValidationRule<CustomerInfo>(x => x.LastName.Length > 0, "Last Name is required.")
            };
        }

        /// <summary>
        /// FirstName of customers
        /// </summary>
        public string FirstName { get; set; } = Defaults.String;

        /// <summary>
        /// MiddleName of customer
        /// </summary>
        public string MiddleName { get; set; } = Defaults.String;

        /// <summary>
        /// LastName of customer
        /// </summary>
        public string LastName { get; set; } = Defaults.String;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public DateTime BirthDate { get; set; } = Defaults.Date;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public int GenderId { get; set; } = Genders.NotSet.Key;

        /// <summary>
        /// Type of customer
        /// </summary>
        public int CustomerTypeId { get; set; } = Defaults.Integer;

        /// <summary>
        /// ISO 5218 Standard for Gender values
        /// </summary>
        public struct Genders
        {
            /// <summary>
            /// Default. Not set
            /// </summary>
            public static KeyValuePair<int, string> NotSet { get; } = new KeyValuePair<int, string>(-1, "Not Set");

            /// <summary>
            /// Unknown gender
            /// </summary>
            public static KeyValuePair<int, string> NotKnown { get; } = new KeyValuePair<int, string>(0, "Not Known");

            /// <summary>
            /// Male gender
            /// </summary>
            public static KeyValuePair<int, string> Male { get; } = new KeyValuePair<int, string>(1, "Male");

            /// <summary>
            /// Femal Gender
            /// </summary>
            public static KeyValuePair<int, string> Female { get; } = new KeyValuePair<int, string>(2, "Female");

            /// <summary>
            /// Not applicable or do not want to specify
            /// </summary>
            public static KeyValuePair<int, string> NotApplicable { get; } = new KeyValuePair<int, string>(9, "Not Applicable");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerInfo() : base() { }

        /// <summary>
        /// Gets all records that equal first + last + birth date
        /// Does == style search
        /// </summary>
        /// <param name="firstName">First name of customer</param>
        /// <param name="lastName">Last Name of customer</param>
        /// <param name="birthDate">Birth Date of customer</param>
        /// <returns></returns>
        public static IQueryable<CustomerInfo> GetByNameBirthdayKey(string firstName, string lastName, DateTime birthDate)
        {
            var reader = new EntityReader<CustomerInfo>();
            IQueryable<CustomerInfo> returnValue = reader.GetAll()
                .Where(x => (firstName != Defaults.String && x.FirstName == firstName)
                && (lastName != Defaults.String && x.LastName == lastName)
                && (birthDate != Defaults.Date && x.BirthDate == birthDate));
            return returnValue;
        }

        /// <summary>
        /// Gets all records that contain any of the passed fields.
        /// Does contains/like style search
        /// </summary>
        /// <param name="searchFields">ICustomer with data to search</param>
        /// <returns>All records matching the passed ICustomer</returns>
        public static IQueryable<CustomerInfo> GetByAny(ICustomer searchFields)
        {
            var reader = new EntityReader<CustomerInfo>();
            IQueryable<CustomerInfo> returnValue = reader.GetAll()
                .Where(x => (searchFields.FirstName != Defaults.String && x.FirstName.Contains(searchFields.FirstName))
                || (searchFields.LastName != Defaults.String && x.LastName.Contains(searchFields.LastName))
                || (searchFields.BirthDate != Defaults.Date && x.BirthDate == searchFields.BirthDate)
                || (x.Id == searchFields.Id));
            return returnValue;
        }

        /// <summary>
        /// Read data based on the passed expression
        /// </summary>
        /// <param name="expression">Expression to apply to the dataset, such as a where clause</param>
        /// <returns></returns>
        public IQueryable<CustomerInfo> Read(Expression<Func<CustomerInfo, bool>> expression)
        {
            var reader = new EntityReader<CustomerInfo>();
            var returnValue = reader.GetByWhere(expression);
            return returnValue;
        }

        /// <summary>
        /// Save the entity to the database. This method will auto-generate activity tracking.
        /// </summary>
        public CustomerInfo Save()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            // Ensure data does not contain cross site scripting injection HTML/Js/SQL
            FirstName = new HtmlUnsafeCleanser(FirstName).Cleanse();
            MiddleName = new HtmlUnsafeCleanser(MiddleName).Cleanse();
            LastName = new HtmlUnsafeCleanser(LastName).Cleanse();
            this.Fill(writer.Save(this));
            return this;
        }

        /// <summary>
        /// Save the entity to the database.
        /// This method requires a valid Activity to track this database commit
        /// </summary>
        /// <param name="activity">Activity tracking this record</param>
        public CustomerInfo Save(IActivityContext activity)
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            ActivityContextKey = activity.ActivityContextKey;
            // Ensure data does not contain cross site scripting injection HTML/Js/SQL
            FirstName = new HtmlUnsafeCleanser(FirstName).Cleanse();
            MiddleName = new HtmlUnsafeCleanser(MiddleName).Cleanse();
            LastName = new HtmlUnsafeCleanser(LastName).Cleanse();
            this.Fill(writer.Save(this));
            return this;
        }

        /// <summary>
        /// Save the entity to the database. This method will auto-generate activity tracking.
        /// </summary>
        public CustomerInfo Delete()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            this.Fill(writer.Delete(this));
            return this;
        }
    }
}
