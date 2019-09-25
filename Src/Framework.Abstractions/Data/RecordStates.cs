//-----------------------------------------------------------------------
// <copyright file="RecordState.cs" company="GoodToCode">
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
using System;

namespace GoodToCode.Framework.Data
{
    /// <summary>
    /// Status of the entity current state. Can be multiple values to reduce collisions and maintain independent behavior on a per-value basis.
    /// </summary>
    /// <remarks></remarks>
    public struct RecordStates
    {
        /// <summary>
        /// Normal behavior: Allows all querying and changes.
        /// </summary>
        public static Guid Default = Defaults.Guid;

        /// <summary>
        /// ReadOnly/Locked: Do not allow to be changed. Ignore and log any change request. Alert calling app that record is read only (can be changed back to default to be altered later, not historical.)
        /// </summary>
        public static Guid ReadOnly = new Guid("F3B57E0D-9213-425C-B86B-405E46EB37AA");

        /// <summary>
        /// Record now historical. This record can never be updated, and will now be excluded out of all re-calculations (becomes a line item to feed historical counts.)
        /// </summary>
        public static Guid Historical = new Guid("5A5DAEB7-235A-4E00-9AAB-99C1D96ED5B5");

        /// <summary>
        /// Deleted: This record is deleted and to be considered non-existent, even in historical re-calculations (will make historical counts shift.)
        /// </summary>
        public static Guid Deleted = new Guid("081C6A5B-0817-4161-A3AD-AD7924BEA874");
    }
}
