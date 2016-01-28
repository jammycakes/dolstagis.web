using System;
using System.Linq;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Logging;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;
using StructureMap;

namespace Dolstagis.Web
{
    /// <summary>
    ///  A container for services and configuration for one part of the application.
    ///  Features can be enabled or disabled as required, and last for the duration
    ///  of the application lifecycle.
    /// </summary>

    public abstract class Feature : IFeature
    {
        private static readonly Logger log = Logger.ForThisClass();

        private bool _constructed = false;
        private readonly FeatureSwitch _switch = new FeatureSwitch();
        private string _description = null;

        private void AssertConstructing()
        {
            if (_constructed)
                throw new InvalidOperationException("Features can only be configured in the constructor.");
        }


        /* ====== Fluent API ====== */

        /*
         * Features are intended to be immutable once constructed.
         *
         * The fluent configuration is intended to be accessible only from the
         * feature's constructors. Therefore, the fluent configuration methods
         * must be declared as protected. THey must also call AssertConstructing()
         * to ensure that they are only called from the constructor.
         */


        /// <summary>
        ///  Defines the condition under which the feature is active.
        /// </summary>

        protected ISwitchExpression Active
        {
            get {
                AssertConstructing();
                return _switch;
            }
        }


        /// <summary>
        ///  Sets the feature's description.
        /// </summary>
        /// <param name="description"></param>

        protected void Description(string description)
        {
            AssertConstructing();
            _description = description;
        }


        /* ====== Public properties and methods ====== */

        /*
         * The feature's state is only intended to be accessible from external
         * classes, specifically, the rest of the framework.
         * 
         * To avoid polluting IntelliSense with irrelevancies, these must be
         * declared as members of tie IFeature interface, which must be
         * implemented explicitly as shown below.
         *
         * They must also set _constructed = true to ensure that they signal
         * that the feature has been initialised.
         */

        /// <summary>
        ///  Gets the feature switch which controls this feature.
        /// </summary>

        IFeatureSwitch IFeature.Switch {
            get {
                _constructed = true;
                return _switch;
            }
        }


        /// <summary>
        ///  Gets the text description of the feature.
        /// </summary>

        string IFeature.Description {
            get {
                _constructed = true;
                return _description ?? this.GetType().FullName;
            }
        }

        #region /* ====== Old API, being replaced with the new fluent API ====== */


        /// <summary>
        ///  Gets or sets the <see cref="IModelBinder"/> instance used to invoke
        ///  actions provided by this feature.
        /// </summary>

        public IModelBinder ModelBinder { get; set; }

        /// <summary>
        ///  Gets the StructureMap Registry for services defined by this feature
        /// </summary>

        public Registry Services { get; private set; }


        /// <summary>
        ///  Gets or sets the table of routes defined by this feature.
        /// </summary>

        public IRouteTable Routes { get; set; }


        /// <summary>
        ///  Creates a new instance of this feature.
        /// </summary>

        protected Feature()
        {
            this.Routes = new RouteTable(this);
            this.Services = new Registry();
            this.ModelBinder = Dolstagis.Web.ModelBinding.ModelBinder.Default;
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

        #endregion
    }
}
