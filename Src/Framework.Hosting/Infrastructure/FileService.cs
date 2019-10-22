
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// File IO service
    /// </summary>
    public static partial class FileServicesExtensions
    {
        /// <summary>
        /// Adds file service
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFileIo(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            return services.AddSingleton<IFileService, FileService>();
        }
    }

    /// <summary>
    /// File IO interface
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Saves byte array to a file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(string path, string file, byte[] content);
    }

    /// <summary>
    /// File IO Service
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Cons
        /// </summary>
        /// <param name="environment"></param>
        public FileService(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        /// <summary>
        /// Saves to file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(string path, string file, byte[] content)
        {
            Directory.CreateDirectory(Path.Combine(path));
            File.AppendAllText(Path.Combine(path, file), content.ToString());
            return await Task.Run(() => true);
        }
    }
}