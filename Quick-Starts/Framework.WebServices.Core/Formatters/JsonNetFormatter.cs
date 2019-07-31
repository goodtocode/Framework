//-----------------------------------------------------------------------
// <copyright file="JsonNetFormatter.cs" company="GoodToCode">
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
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using GoodToCode.Extensions;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Framework.WebServices
{
    /// <summary>
    /// Adds JSON.NET formatter to handle ISO 8601 dates
    /// Use: Startup.cs
    ///      services.AddMvc(options =>
    ///     {   options.InputFormatters.Insert(0, new JsonNetFormatterCustom());
    ///         options.OutputFormatters.Insert(0, new JsonNetFormatterCustom());
    ///     }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    /// </summary>
    public class JsonNetFormatterCustom : MediaTypeFormatter, IInputFormatter, IOutputFormatter
    {
        private readonly JsonSerializerSettings jsonSerializerSettingsValue;
        private readonly Encoding utf8Encoding = new UTF8Encoding(false, true);
        private readonly MediaTypeHeaderValue jsonMediaType = new MediaTypeHeaderValue("application/json");

        /// <summary>
        /// Constructor
        /// </summary>
        public JsonNetFormatterCustom() : base()
        {
            SupportedMediaTypes.Add(jsonMediaType);
            SupportedEncodings.Add(utf8Encoding);
        }

        /// <summary>
        /// Constructor
        /// Derives from System.Net.Http.Formatting.MediaTypeFormatter
        /// </summary>
        /// <param name="jsonSerializerSettings"></param>
        public JsonNetFormatterCustom(JsonSerializerSettings jsonSerializerSettings) : this()
        {
            jsonSerializerSettingsValue = jsonSerializerSettings ?? new JsonSerializerSettings();
        }

        /// <summary>
        /// Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can deserialize
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool CanRead(InputFormatterContext context)
        {
            return true;
        }

        /// <summary>
        /// Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can deserialize
        /// object of the specified type
        /// </summary>
        /// <param name="type">The type to deserialize</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can deserialize the type; otherwise, false.</returns>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        ///     Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can serializean
        ///     object of the specified type.
        /// </summary>
        /// <param name="context">The type to serialize</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can serialize the type; otherwise, false.</returns>
        public bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            return true;
        }

        /// <summary>
        ///     Queries whether this System.Net.Http.Formatting.MediaTypeFormatter can serializean
        ///     object of the specified type.
        /// </summary>
        /// <param name="type">The type to serialize</param>
        /// <returns>true if the System.Net.Http.Formatting.MediaTypeFormatter can serialize the type; otherwise, false.</returns>
        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <param name="context">Context containing information required</param>
        /// <returns>A System.Threading.Tasks.Task whose result will be an object of the given type</returns>
        public async Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);

            try
            {
                using (var streamReader = new StreamReader(context.HttpContext.Request.Body, utf8Encoding))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return await InputFormatterResult.SuccessAsync(serializer.Deserialize(jsonTextReader));
                    }
                }
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        /// <summary>
        /// Asynchronously deserializes an object of the specified type.
        /// </summary>
        /// <param name="type">The type of the object to deserialize</param>
        /// <param name="readStream">The System.IO.Stream to read</param>
        /// <param name="content">The System.Net.Http.HttpContent, if available. It may be null</param>
        /// <param name="formatterLogger">The System.Net.Http.Formatting.IFormatterLogger to log events to</param>
        /// <returns>A System.Threading.Tasks.Task whose result will be an object of the given type</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);

            return Task.Factory.StartNew(() =>
            {
                using (var streamReader = new StreamReader(readStream, utf8Encoding))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        return serializer.Deserialize(jsonTextReader, type);
                    }
                }
            });
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type
        /// </summary>
        /// <param name="context">The type of the object to write</param>
        /// <returns>A System.Threading.Tasks.Task that will perform the write</returns>
        public Task WriteAsync(OutputFormatterWriteContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);
            
            return Task.Factory.StartNew(() =>
            {
                using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(context.HttpContext.Request.Body, utf8Encoding)) { DateFormatString = DateTimeExtension.Formats.ISO8601F, CloseOutput = false })
                {
                    serializer.Serialize(jsonTextWriter, context.Object);
                    jsonTextWriter.Flush();
                }
            });
        }

        /// <summary>
        /// Asynchronously writes an object of the specified type
        /// </summary>
        /// <param name="type">The type of the object to write</param>
        /// <param name="value">The object value to write. It may be null</param>
        /// <param name="writeStream">The System.IO.Stream to which to write</param>
        /// <param name="content">The System.Net.Http.HttpContent if available. It may be null</param>
        /// <param name="transportContext">The System.Net.TransportContext if available. It may be null</param>
        /// <returns>A System.Threading.Tasks.Task that will perform the write</returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            var serializer = JsonSerializer.Create(jsonSerializerSettingsValue);

            return Task.Factory.StartNew(() =>
            {
                using (var jsonTextWriter = new JsonTextWriter(new StreamWriter(writeStream, utf8Encoding)) { DateFormatString = DateTimeExtension.Formats.ISO8601F, CloseOutput = false })
                {
                    serializer.Serialize(jsonTextWriter, value);
                    jsonTextWriter.Flush();
                }
            });
        }
    }
}


