//-----------------------------------------------------------------------
// <copyright file="BitmapExtensions.cs" company="GoodToCode">
//      Copyright (c) 2017-2018 GoodToCode. All rights reserved.
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
using System.Drawing;
using System.Windows.Media.Imaging;

namespace GoodToCode.Framework.Windows
{
    /// <summary>
    /// Default page in WPF template
    /// </summary>
    public static class BitmapExtensions
    {
        /// <summary>
        /// Converts Bitmap to BitmapImage
        /// </summary>
        /// <param name="item">Bitmap to be converted</param>
        /// <returns>BitmapImage of the passed Bitmap</returns>
        public static BitmapImage ToBitmapImage(this Bitmap item)
        {
            var returnValue = new BitmapImage
            {
                StreamSource = new System.IO.MemoryStream(item.ToBytes())
            };
            return returnValue;
        }
    }
}
