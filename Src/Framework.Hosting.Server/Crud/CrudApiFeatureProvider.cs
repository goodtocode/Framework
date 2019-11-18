using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GoodToCode.Framework.Hosting.Server
{
    /// <summary>
    /// Adds feature
    /// Example usage: 
    ///   public void ConfigureServices(IServiceCollection services) {
    ///      services.
    ///          AddMvc(o => o.Conventions.Add(
    ///              new CrudApiControllerRouteConvention()
    ///          )).
    ///          ConfigureApplicationPartManager(m =>
    ///              m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()
    ///          ));}
    /// </summary>
    public class CrudApiControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly List<CrudApiRoute> typesAndRoutes = new List<CrudApiRoute>();

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiControllerFeatureProvider() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="routeToBind"></param>
        public CrudApiControllerFeatureProvider(Type entityType, string routeToBind)
        {
            typesAndRoutes.Add(new CrudApiRoute(entityType, routeToBind));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllerTypesAndRoutes"></param>
        public CrudApiControllerFeatureProvider(List<CrudApiRoute> controllerTypesAndRoutes)
        {
            typesAndRoutes.AddRange(controllerTypesAndRoutes);
        }

        /// <summary>
        /// Adds the type and controller, without the Generic aspect of CrudApiController
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="feature"></param>
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var item in typesAndRoutes)
            {
                feature.Controllers.Add(
                    typeof(CrudApiController<>).MakeGenericType(item.CrudType).GetTypeInfo()
                );
            }
        }
    }
}

