using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Dolstagis.Web
{
    /// <summary>
    ///  A container for services and configuration for one part of the application.
    ///  Features can be enabled or disabled as required, and last for the duration
    ///  of the application lifecycle.
    /// </summary>

    public class Feature
    {
        /// <summary>
        ///  Gets the StructureMap Registry for services defined by this feature
        /// </summary>

        public Registry Services { get; private set; }


        /// <summary>
        ///  Gets the text description of the feature.
        /// </summary>

        public virtual string Description { get { return this.GetType().FullName; } }


        /// <summary>
        ///  Gets or sets the table of routes defined by this feature.
        /// </summary>

        public IRouteTable Routes { get; set; }


        #region /* ====== Implementation of IRouteRegistry ====== */

        public IList<Dolstagis.Web.Routing.IRouteDefinition> LegacyRoutes { get; private set; }

        #endregion

        /// <summary>
        ///  Creates a new instance of this feature.
        /// </summary>

        public Feature()
        {
            this.Routes = new RouteTable();
            this.LegacyRoutes = new List<Dolstagis.Web.Routing.IRouteDefinition>();
            this.Services = new Registry();
        }


        /// <summary>
        ///  Registers a <see cref="Handler"/> in this feature by type,
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
                AddHandler<T>(attr.Route);
            }
        }

        /// <summary>
        ///  Registers a <see cref="Handler"/> in this feature with a specified route.
        /// </summary>
        /// <typeparam name="T">
        ///  The type of this handler.
        /// </typeparam>
        /// <param name="route">
        ///  The route definition for this handler.
        /// </param>

        public void AddHandler<T>(string route) where T: Handler
        {
            this.Routes.Add(route, new RouteTarget(typeof(T)));
            this.LegacyRoutes.Add(new Dolstagis.Web.Routing.RouteDefinition(typeof(T), route, this, x => true));
        }

        /* ====== AddStaticFiles and AddViews helper methods ====== */

        protected void AddStaticResources(ResourceType type, VirtualPath vPath, Func<IContext, IResourceLocation> locationFactory)
        {
            Services.For<ResourceMapping>()
                .Add(ctx => new ResourceMapping(type, vPath, locationFactory(ctx)));
        }

        protected void AddStaticResources(ResourceType type, VirtualPath vPath, IResourceLocation location)
        {
            Services.For<ResourceMapping>().Add(ctx => new ResourceMapping(type, vPath, location));
        }

        protected void AddStaticResources(ResourceType type, VirtualPath path, string physicalPath)
        {
            AddStaticResources(type, path, new FileResourceLocation(physicalPath));
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
            AddStaticResources(ResourceType.StaticFiles, vPath, locationFactory);
            AddStaticFilesHandler(vPath);
        }


        public void AddStaticFiles(string path, Func<IContext, IResourceLocation> locationFactory)
        {
            AddStaticFiles(new VirtualPath(path), locationFactory);
        }

        /// <summary>
        ///  Registers a directory or file of static files at the specified location.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="location"></param>

        public void AddStaticFiles(VirtualPath vPath, IResourceLocation location)
        {
            AddStaticResources(ResourceType.StaticFiles, vPath, location);
            AddStaticFilesHandler(vPath);
        }

        /// <summary>
        ///  Registers a directory or file of static files at the specified location.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>

        public void AddStaticFiles(string path, IResourceLocation location)
        {
            AddStaticFiles(new VirtualPath(path), location);
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
            AddStaticResources(ResourceType.StaticFiles, path, physicalPath);
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
            AddStaticResources(ResourceType.Views, vPath, locationFactory);
        }

        /// <summary>
        ///  Registers a directory or file of views,
        ///  using a location created with dependencies taken from the IOC container.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="locationFactory"></param>

        public void AddViews(string path, Func<IContext, IResourceLocation> locationFactory)
        {
            AddViews(new VirtualPath(path), locationFactory);
        }

        /// <summary>
        ///  Registers a directory or file of views at the specified location.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="location"></param>

        public void AddViews(VirtualPath vPath, IResourceLocation location)
        {
            AddStaticResources(ResourceType.Views, vPath, location);
        }

        /// <summary>
        ///  Registers a directory or file of views at the specified location.
        /// </summary>
        /// <param name="vPath"></param>
        /// <param name="location"></param>

        public void AddViews(string path, IResourceLocation location)
        {
            AddViews(new VirtualPath(path), location);
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
            AddStaticResources(ResourceType.Views, path, physicalPath);
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
