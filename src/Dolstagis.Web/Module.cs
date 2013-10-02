using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Views;
using Dolstagis.Web.Views.Static;
using StructureMap.Configuration.DSL;

namespace Dolstagis.Web
{
    /// <summary>
    ///  A container for services and configuration for one part of the application.
    ///  Modules can be enabled or disabled as required, and last for the duration
    ///  of the application lifecycle.
    /// </summary>

    public class Module : IRouteRegistry
    {
        public Registry Services { get; private set; }

        /// <summary>
        ///  Gets the registry of HTML templates.
        /// </summary>

        public ResourceLocator Templates { get; private set; }

        /// <summary>
        ///  Gets the registry of static files.
        /// </summary>

        public ResourceLocator StaticFiles { get; private set; }

        /// <summary>
        ///  Gets the text description of the module.
        /// </summary>

        public virtual string Description { get { return this.GetType().FullName; } }

        /// <summary>
        ///  Gets or sets a value indicating whether the module is enabled.
        /// </summary>

        public bool Enabled { get; set; }

        /// <summary>
        ///  Creates a new instance of this module.
        /// </summary>

        #region /* ====== Implementation of IRouteRegistry ====== */

        public IList<IRouteDefinition> Routes { get; private set; }

        #endregion

        public Module()
        {
            this.Enabled = true;
            this.Routes = new List<IRouteDefinition>();
            this.Services = new Registry();
            this.Templates = new ResourceLocator();
            this.StaticFiles = new ResourceLocator();
        }

        /// <summary>
        ///  Registers a <see cref="Handler"/> in this module by type,
        ///  with a route specified in a [Route] attribute on the handler
        ///  class declaration.
        /// </summary>
        /// <typeparam name="T"></typeparam>

        public void AddHandler<T>() where T: Handler
        {
            var attributes = typeof(T).GetCustomAttributes(typeof(RouteAttribute), true);
            if (!attributes.Any()) {
                throw new ArgumentException("Type " + typeof(T) + " does not declare any routes, and no route was specified.");
            }

            foreach (RouteAttribute attr in attributes) {
                AddHandler<T>(attr.Route, x => true);
            }
        }

        /// <summary>
        ///  Registers a <see cref="Handler"/> in this module with a specified route.
        /// </summary>
        /// <typeparam name="T">
        ///  The type of this handler.
        /// </typeparam>
        /// <param name="route">
        ///  The route definition for this handler.
        /// </param>

        public void AddHandler<T>(string route) where T: Handler
        {
            AddHandler<T>(route, x => true);
        }

        /// <summary>
        ///  Registers a <see cref="Handler"/> in this module with a specified route
        ///  and precondition.
        /// </summary>
        /// <typeparam name="T">
        ///  The type of this handler.
        /// </typeparam>
        /// <param name="route">
        ///  The route definition for this handler.
        /// </param>
        /// <param name="precondition">
        ///  A function giving a precondition whether this route is valid or not.
        /// </param>

        public void AddHandler<T>(string route, Func<RouteInfo, bool> precondition) where T: Handler
        {
            this.Routes.Add(new RouteDefinition(typeof(T), route, this, precondition));
        }


        public void AddStaticFiles(string path)
        {
            path = path.NormaliseUrlPath();
            string route = path + "/{path+}";
            AddHandler<StaticHandler>(route);
            this.Services.For<IResourceLocation>().Singleton().Use(new FilespaceResourceLocation(path));
        }
    }
}
