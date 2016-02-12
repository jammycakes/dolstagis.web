using System;

namespace Dolstagis.Web.Features
{
    public interface IRouteExpression
    {
        /// <summary>
        ///  Adds a route from the specified path to a controller or provider of
        ///  static files.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        IRouteFromExpression From(VirtualPath path);

        /// <summary>
        ///  Adds a route to the controller of the specified type. The controller
        ///  must be marked with the [Route] attribute.
        /// </summary>
        /// <param name="controllerType"></param>

        void Controller(Type controllerType);

        /// <summary>
        ///  Adds a route to the controller of the specified type. The controller
        ///  must be marked with the [Route] attribute.
        /// </summary>
        /// <typeparam name="TController"></typeparam>

        void Controller<TController>();

        /// <summary>
        ///  Scans the assembly containing the current feature and registers
        ///  routes to all controllers that it finds marked with the [Route]
        ///  attribute.
        /// </summary>

        void AllControllersInAssembly();
    }
}
