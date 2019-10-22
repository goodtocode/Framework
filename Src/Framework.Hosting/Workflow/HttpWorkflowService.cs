
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// Service extension
    /// </summary>
    public static partial class HttpWorkflowServiceExtension
    {
        /// <summary>
        /// Adds workflow
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHttpWorkflow(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services.AddScoped<IFileService, FileService>();
        }
    }

    /// <summary>
    /// Service interace
    /// </summary>
    public interface IHttpWorkflowService
    {
        /// <summary>
        /// Processes the workflow
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> ProcessAsync(string path, string file, byte[] content);
    }

    /// <summary>
    /// Workflow service
    /// </summary>
    public class HttpWorkflowService : IHttpWorkflowService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environment"></param>
        public HttpWorkflowService(IHostingEnvironment environment)
        {
            this.hostingEnvironment = environment;
        }


        /// <summary>
        /// Processes the workflow
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<bool> ProcessAsync(string path, string file, byte[] content)
        {
            return await Task.Run(() => true);
        }
    }
}