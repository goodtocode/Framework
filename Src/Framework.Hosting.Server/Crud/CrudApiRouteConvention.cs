using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodToCode.Framework.Hosting.Server
{
    /// <summary>
    /// Route convetion adding route to controller
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
    public class CrudApiControllerRouteConvention : IControllerModelConvention
    {
        private List<CrudApiRoute> typesAndRoutes = new List<CrudApiRoute>();

        /// <summary>
        /// Constructor
        /// </summary>
        public CrudApiControllerRouteConvention() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="routeToBind"></param>
        public CrudApiControllerRouteConvention(Type entityType, string routeToBind)
        {
            typesAndRoutes.Add(new CrudApiRoute(entityType, routeToBind));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="controllerTypesAndRoutes"></param>
        public CrudApiControllerRouteConvention(List<CrudApiRoute> controllerTypesAndRoutes)
        {
            typesAndRoutes.AddRange(controllerTypesAndRoutes);
        }

        /// <summary>
        /// Applys the route convention
        /// </summary>
        /// <param name="controller"></param>
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                // Method 1 - Default
                var genericType = controller.ControllerType.GenericTypeArguments[0];
                controller.ControllerName = genericType.Name;

                // Method 2 - Route["api/Entity"]
                var route = typesAndRoutes.Where(x => x.CrudType == genericType).Select(y => y.CrudRoute).FirstOrDefault()
                    ?? $"api/{genericType.Name}";
                controller.Selectors.Add(new SelectorModel
                {
                    AttributeRouteModel = new AttributeRouteModel(new RouteAttribute(route)),
                });
            }
        }
    }
}

