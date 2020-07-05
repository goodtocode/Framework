//-----------------------------------------------------------------------
// <copyright file="HttpVerbSender.cs" company="GoodToCode">
//      Copyright (c) 2017-2020 GoodToCode. All rights reserved.
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

using GoodToCode.Extensions.Net;
using System;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Net
{
    /// <summary>
    /// Handles sending HttpRequests to endpoints and receiving a response
    /// </summary>
    public class HttpVerbSender
    {
        /// <summary>
        /// Event Args for loading and binding new model data
        /// </summary>
        public class RequestEventArgs : EventArgs
        {
            /// <summary>
            /// New model data
            /// </summary>
            public HttpRequestClient Request { get; set; }
        }

        /// <summary>
        /// Send is about to begin
        /// </summary>
        public event SendBeginEventHandler SendBegin;

        /// <summary>
        /// Send is complete
        /// </summary>
        public event SendBeginEventHandler SendEnd;

        /// <summary>
        /// OnSendBegin()
        /// </summary>
        public virtual void OnSendBegin(HttpRequestClient request)
        {
            SendBegin?.Invoke(this, new RequestEventArgs() { Request = request });
            Request = request;
        }

        /// <summary>
        /// OnSendEnd()
        /// </summary>
        public virtual void OnSendEnd(HttpRequestClient request)
        {
            SendEnd?.Invoke(this, new RequestEventArgs() { Request = request });
            Request = request;
        }

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Arguments passed to the event handler</param>
        public delegate void SendBeginEventHandler(object sender, RequestEventArgs e);

        /// <summary>
        /// Last HttpRequest
        /// </summary>
        public HttpRequestClient Request { get; protected set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public HttpVerbSender()
            : base()
        {
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDataOut> SendGetAsync<TDataOut>(Uri fullUrl) where TDataOut : new()
        {
            var returnValue = default(TDataOut);
            var request = new HttpRequestGet<TDataOut>(fullUrl);

            OnSendBegin(request);
            returnValue = await request.SendAsync();
            OnSendEnd(request);

            return returnValue;
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDataOut> SendPostAsync<TDataIn, TDataOut>(Uri fullUrl, TDataIn itemToSend)
        {
            var returnValue = default(TDataOut);
            var request = new HttpRequestPost<TDataIn, TDataOut>(fullUrl, itemToSend);

            OnSendBegin(request);
            returnValue = await request.SendAsync();
            OnSendEnd(request);

            return returnValue;
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDataInOut> SendPostAsync<TDataInOut>(Uri fullUrl, TDataInOut itemToSend)
        {
            return await SendPostAsync<TDataInOut, TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDataOut> SendPutAsync<TDataIn, TDataOut>(Uri fullUrl, TDataIn itemToSend)
        {
            var returnValue = default(TDataOut);
            var request = new HttpRequestPut<TDataIn, TDataOut>(fullUrl, itemToSend);

            OnSendBegin(request);
            returnValue = await request.SendAsync();
            OnSendEnd(request);

            return returnValue;
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<TDataInOut> SendPutAsync<TDataInOut>(Uri fullUrl, TDataInOut itemToSend)
        {
            return await SendPutAsync<TDataInOut, TDataInOut>(fullUrl, itemToSend);
        }

        /// <summary>
        /// Instantiates and transmits all data to the middle tier web service that will execute the workflow
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> SendDeleteAsync(Uri fullUrl)
        {
            var returnValue = false;
            var request = new HttpRequestDelete(fullUrl);

            OnSendBegin(request);
            await request.SendAsync();
            OnSendEnd(request);

            return request.Response.IsSuccessStatusCode;
        }
    }
}
