using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Views;
using StructureMap.Configuration.DSL;
using Dolstagis.Web.Static;
using System.IO;
using StructureMap;

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
        ///  Gets the text description of the module.
        /// </summary>

        public virtual string Description { get { return this.GetType().FullName; } }

        /// <summary>
        ///  Gets or sets a value indicating whether the module is enabled.
        /// </summary>

        public bool Enabled { get; set; }

        #region /* ====== Implementation of IRouteRegistry ====== */

        public IList<IRouteDefinition> Routes { get; private set; }

        #endregion

        /// <summary>
        ///  Creates a new instance of this module.
        /// </summary>

        public Module()
        {
            this.Enabled = true;
            this.Routes = new List<IRouteDefinition>();
            this.Services = new Registry();
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

        /* ====== AddStaticFiles and AddViews helper methods ====== */

        private void AddStaticResources(string type, VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory)
        {
            Services.For<ResourceMapping>()
                .Add(ctx => new ResourceMapping(type, vPath, locationFactory(ctx)));
        }

        private void AddStaticResources(string type, VirtualPath vPath, IResourceLocation location)
        {
            Services.For<ResourceMapping>().Add(ctx => new ResourceMapping("StaticFiles", vPath, location));
        }

        private void AddStaticResources(string type, VirtualPath path)
        {
            AddStaticResources(type, path, ctx => new FileResourceLocation
                (ctx.GetInstance<IApplicationContext>().MapPath(path)));
        }

        private void AddStaticResources(string type, VirtualPath path, string physicalPath)
        {
            if (Path.IsPathRooted(physicalPath))
            {
                AddStaticResources(type, path, new FileResourceLocation(physicalPath));
            }
            else
            {
                var vPhysicalPath = new VirtualPath(physicalPath);
                AddStaticResources(type, path, ctx => new FileResourceLocation(
                    ctx.GetInstance<IApplicationContext>().MapPath(vPhysicalPath)));
            }
            AddStaticFilesHandler(path);
        }

        private void AddStaticFilesHandler(VirtualPath vPath)
        {
            AddHandler<StaticRequestHandler>(vPath.Path + "/{path*}");
        }

        /* ====== AddStaticFiles methods ====== */

        /// <summary>
        ///  Registers a directory or file of static files,
        ///  using a location created with dependencies taken from the IOC container.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="locationFactory"></param>

        public void AddStaticFiles(VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory)
        {
            AddStaticResources("StaticFiles", vPath, locationFactory);
            AddStaticFilesHandler(vPath);
        }

        /// <summary>
        ///  Registers a directory or file of static files at the specified location.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="location"></param>

        public void AddStaticFiles(VirtualPath vPath, IResourceLocation location)
        {
            AddStaticResources("StaticFiles", vPath, location);
            AddStaticFilesHandler(vPath);
        }

        /// <summary>
        ///  Registers a directory or file of static files.
        /// </summary>
        /// <param name="path">
        ///  The path to the static file or directory.
        /// </param>

        public void AddStaticFiles(VirtualPath path)
        {
            AddStaticResources("StaticFiles", path);
            AddStaticFilesHandler(path);
        }

        /// <summary>
        ///  Registers a directory or file of static files.
        /// </summary>
        /// <param name="path">
        ///  The path to the static file or directory.
        /// </param>

        public void AddStaticFiles(string path)
        {
            AddStaticFiles(new VirtualPath(path));
        }

        /// <summary>
        ///  Registers a directory or file of static files at a different mapping from
        ///  that within the filespace of the website.
        /// </summary>
        /// <param name="path">
        ///  The virtual path to the static file or directory.
        /// </param>
        /// <param name="physicalPath">
        ///  The physical path to the static file or directory. This may be either relative
        ///  to the application root, or else an absolute path.
        /// </param>

        public void AddStaticFiles(VirtualPath path, string physicalPath)
        {
            AddStaticResources("StaticFiles", path, physicalPath);
            AddStaticFilesHandler(path);
        }

        /// <summary>
        ///  Registers a directory or file of static files at a different mapping from
        ///  that within the filespace of the website.
        /// </summary>
        /// <param name="path">
        ///  The virtual path to the static file or directory.
        /// </param>
        /// <param name="physicalPath">
        ///  The physical path to the static file or directory. This may be either relative
        ///  to the application root, or else an absolute path.
        /// </param>

        public void AddStaticFiles(string path, string physicalPath)
        {
            AddStaticFiles(new VirtualPath(path), physicalPath);
        }

        /* ====== AddViews methods ====== */

        /// <summary>
        ///  Registers a directory or file of views,
        ///  using a location created with dependencies taken from the IOC container.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="locationFactory"></param>

        public void AddViews(VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory)
        {
            AddStaticResources("Views", vPath, locationFactory);
        }

        /// <summary>
        ///  Registers a directory or file of views at the specified location.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="location"></param>

        public void AddViews(VirtualPath vPath, IResourceLocation location)
        {
            AddStaticResources("Views", vPath, location);
        }

        /// <summary>
        ///  Registers a directory or file of view locations.
        /// </summary>
        /// <param name="path">
        ///  The path to the view template or directory.
        /// </param>

        public void AddViews(VirtualPath path)
        {
            AddStaticResources("Views", path);
        }

        /// <summary>
        ///  Registers a directory or file of view locations.
        /// </summary>
        /// <param name="path">
        ///  The path to the view template or directory.
        /// </param>

        public void AddViews(string path)
        {
            AddViews(new VirtualPath(path));
        }

        /// <summary>
        ///  Registers a directory or file of static files at a different mapping from
        ///  that within the filespace of the website.
        /// </summary>
        /// <param name="path">
        ///  The virtual path to the static file or directory.
        /// </param>
        /// <param name="physicalPath">
        ///  The physical path to the static file or directory. This may be either relative
        ///  to the application root, or else an absolute path.
        /// </param>

        public void AddViews(VirtualPath path, string physicalPath)
        {
            AddStaticResources("Views", path, physicalPath);
        }

        /// <summary>
        ///  Registers a directory or file of static files at a different mapping from
        ///  that within the filespace of the website.
        /// </summary>
        /// <param name="path">
        ///  The virtual path to the static file or directory.
        /// </param>
        /// <param name="physicalPath">
        ///  The physical path to the static file or directory. This may be either relative
        ///  to the application root, or else an absolute path.
        /// </param>

        public void AddViews(string path, string physicalPath)
        {
            AddViews(new VirtualPath(path), physicalPath);
        }
    }
}
