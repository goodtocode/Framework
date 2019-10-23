using GoodToCode.Framework.Data;
using System;

namespace GoodToCode.Framework.Hosting.Server
{
    /// <summary>
    /// Crud Controller and Route information
    /// </summary>
    public class CrudApiRoute
    {
        /// <summary>
        /// Type representing the CrudApiController
        /// </summary>
        public Type CrudType { get; set; }

        /// <summary>
        /// Route associated with this type
        /// </summary>
        public string CrudRoute { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiRoute() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="routeToBind"></param>
        public CrudApiRoute(Type entityType, string routeToBind)
        {
            CrudType = entityType;
            CrudRoute = routeToBind;
        }
    }
}