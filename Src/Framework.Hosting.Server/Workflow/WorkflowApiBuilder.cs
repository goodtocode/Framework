using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;

namespace GoodToCode.Framework.Hosting
{
    /// <summary>
    /// Extension methods for <see cref="IApplicationBuilder"/> to add Mvc to the request execution pipeline.
    /// </summary>
    public static partial class BuilderExtensions
    {
        /// <summary>
        /// Adds Mvc to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <paramref name="app"/>.</returns>
        /// <remarks>This method only supports attribute routing. To add conventional routes use
        /// <see cref="UseHttpWorkflowApi(IApplicationBuilder, Action{IRouteBuilder})"/>.</remarks>
        public static IApplicationBuilder UseHttpWorkflowApi(this IApplicationBuilder app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            return app.UseHttpWorkflowApi(routes =>
            {
            });
        }

        /// <summary>
        /// Adds Mvc to the <see cref="IApplicationBuilder"/> request execution pipeline
        /// with a default route.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <paramref name="app"/>.</returns>
        public static IApplicationBuilder UseHttpWorkflowApiWithDefaultRoute(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseHttpWorkflowApi(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Adds Mvc to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <param name="configureRoutes">A callback to configure Mvc routes.</param>
        /// <returns>The <paramref name="app"/>.</returns>
        public static IApplicationBuilder UseHttpWorkflowApi(
            this IApplicationBuilder app,
            Action<IRouteBuilder> configureRoutes)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));
            if (configureRoutes is null)
                throw new ArgumentNullException(nameof(configureRoutes));

            //// Verify if AddMvc was done before calling UseHttpWorkflowApi
            //// We use the MvcMarkerService to make sure if all the services were added.
            //MvcServicesHelper.ThrowIfMvcNotRegistered(app.ApplicationServices);

            //var routes = new RouteBuilder(app, new RouteHandler())
            //{
            //    DefaultHandler = new MvcRouteHandler(),
            //    ServiceProvider = app.ApplicationServices
            //};

            //configureRoutes(routes);

            //// Adding the attribute route comes after running the user-code because
            //// we want to respect any changes to the DefaultHandler.
            //routes.Routes.Insert(0, AttributeRouting.CreateAttributeMegaRoute(
            //    routes.DefaultHandler,
            //    app.ApplicationServices));

            return app; //.UseRouter(routes.Build());
        }
    }
}