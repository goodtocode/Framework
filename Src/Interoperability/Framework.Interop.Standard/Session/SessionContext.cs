//-----------------------------------------------------------------------
// <copyright file="SessionContext.cs" company="GoodToCode">
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
using System;
using GoodToCode.Extensions;

namespace GoodToCode.Framework.Session
{
    /// <summary>
    /// Context identity that includes user identity info (user name), application Id and entityID
    /// </summary>
    public class SessionContext : ISessionContext
    {
        /// <summary>
        /// Universally Unique Id (UuId) of the device. Typically same as IMEI number, or DeviceId from the OS
        /// </summary>
        public string DeviceUuid { get; set; } = Defaults.String;

        /// <summary>
        /// Universally Unique Id (UuId) of the software application, that identifies this Application + Device combination
        /// </summary>
        public string ApplicationUuid { get; set; } = Defaults.String;

        /// <summary>
        /// Entity (business or person)
        /// </summary>
        public Guid EntityKey { get; set; } = Defaults.Guid;

        /// <summary>
        /// Name, typically user name
        /// </summary>
        public string IdentityUserName { get; set; } = Defaults.String;

        /// <summary>
        /// Constructor
        /// </summary>
        public SessionContext() : base() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceUuid">Device Id sending request</param>
        /// <param name="applicationUuid">Application Id sending request</param>
        /// <param name="identityUserName">Name of user/authentication name sending request</param>
        public SessionContext(string deviceUuid, string applicationUuid, string identityUserName) : this()
        {
            DeviceUuid = deviceUuid;
            ApplicationUuid = applicationUuid;
            IdentityUserName = identityUserName;            
        }
                
    }
}
