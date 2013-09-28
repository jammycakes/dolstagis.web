using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web
{
    /// <summary>
    ///  A container for services and configuration for one part of the application.
    ///  Modules can be enabled or disabled as required, and last for the duration
    ///  of the application lifecycle.
    /// </summary>

    public class Module
    {
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

        public Module()
        {
            this.Enabled = true;
        }

        /// <summary>
        ///  Registers a <see cref="Handler"/> in this module by type,
        ///  with a route specified in a [Route] attribute on the handler
        ///  class declaration.
        /// </summary>
        /// <typeparam name="T"></typeparam>

        public void AddHandler<T>() where T: Handler
        {
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
        }
    }
}
