using GoodToCode.Extensions;
using GoodToCode.Extensions.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// Extensions to IServiceCollection
    /// </summary>
    public static partial class HttpCrudServicesExtensions
    {
        /// <summary>
        /// Adds Http-based CRUD services to .NET Core Dependency Injection
        /// </summary>
        /// <typeparam name="TDto"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpCrud<TDto>(this IServiceCollection services) where TDto : new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services.AddTransient<IHttpCrudService<TDto>, HttpCrudService<TDto>>();
        }
    }

    /// <summary>
    /// Provides Http-based CRUD services based on:
    ///  1. A single set of RESTful endpoints (i.e. configuration["AppSettings:MyWebService"])
    ///  2. A single Type of Dto in requests/responses
    /// </summary>
    /// <typeparam name="TDto">Type of Dto in requests/responses</typeparam>
    public class HttpCrudService<TDto> : IHttpCrudService<TDto> where TDto : new()
    {
        /// <summary>
        /// Name of the type that is mapped to the query
        /// </summary>
        public string TypeName { get { return typeof(TDto).Name; } }

        /// <summary>
        /// Uri of the CRUD RESTful endpoint
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Response from the request
        /// </summary>
        public HttpResponseMessage Response { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoints"></param>
        public HttpCrudService(IOptions<HttpCrudOptions> endpoints)
        {
            if (endpoints.Value?.Count > 0)
                Uri = endpoints.Value.Find(x => x.Type == TypeName).Url.TryParseUri();
        }

        /// <summary>
        /// Creates an item in the system
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Created item, with updated Id/Key (if applicable)</returns>
        public async Task<TDto> CreateAsync(TDto item)
        {
            TDto returnData;
            using (var client = new HttpRequestPut<TDto>(Uri, item))
            {
                returnData = await client.SendAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="query">Querystring parameters that will result in one item returned</param>
        /// <returns>Item from the system</returns>
        public async Task<TDto> ReadAsync(string query)
        {
            TDto returnData;
            query = query.Replace("//", "/ /");
            var uriQuery = new Uri($"{Uri.ToString().RemoveLast("/")}{query.AddFirst("/")}");
            using (var client = new HttpRequestGet<TDto>(uriQuery))
            {
                returnData = await client.SendAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="id">Id of the item to return</param>
        /// <returns>Item from the system</returns>
        public async Task<TDto> ReadAsync(int id)
        {
            TDto returnData;
            var uriId = new Uri($"{Uri.ToString().RemoveLast("/")}/{id.ToString()}");
            using (var client = new HttpRequestGet<TDto>(uriId))
            {
                returnData = await client.SendAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="key">Key of the item to return</param>
        /// <returns>Item from the system</returns>
        public async Task<TDto> ReadAsync(Guid key)
        {
            TDto returnData;
            var uriKey = new Uri($"{Uri.ToString().RemoveLast("/")}/{key.ToString()}");
            using (var client = new HttpRequestGet<TDto>(uriKey))
            {
                returnData = await client.SendAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }

        /// <summary>
        /// Reads an item from the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="item">item to update</param>
        /// <returns>Item from the system</returns>
        public async Task<TDto> UpdateAsync(TDto item)
        {
            TDto returnData;
            using (var client = new HttpRequestPost<TDto>(Uri, item))
            {
                returnData = await client.SendAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }

        /// <summary>
        /// Deletes an item in the system
        /// Constrained to 1 item. Search using Queryflow
        /// </summary>
        /// <param name="item">item to delete</param>
        /// <returns>Item from the system</returns>
        public async Task<bool> DeleteAsync(TDto item)
        {
            bool returnData;
            using (var client = new HttpRequestDelete(Uri))
            {
                returnData = await client.DeleteAsync();
                Response = client.Response;
            }
            return await Task.Run(() => returnData);
        }
    }
}