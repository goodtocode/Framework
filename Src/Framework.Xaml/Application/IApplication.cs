using GoodToCode.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace GoodToCode.Framework.Application
{
    /// <summary>
    /// Global application information
    /// </summary>
    public interface IApplication : IFrame
    {
        /// <summary>
        /// MyWebService
        /// </summary>
        Uri MyWebService { get; }
    }
}
