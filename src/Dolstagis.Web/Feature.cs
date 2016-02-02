using System;
using System.Linq;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Logging;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;

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
        private RouteTable _routes = new RouteTable();
        private readonly ContainerConfiguration _containerConfiguration
            = new ContainerConfiguration();

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


        protected IContainerExpression Container
        {
            get {
                AssertConstructing();
                return _containerConfiguration;
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


        /// <summary>
        ///  Provides a fluent interface to configure routes to controllers.
        /// </summary>

        protected IRouteExpression Route(VirtualPath specification)
        {
            AssertConstructing();
            return _routes.Add(specification);
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


        /// <summary>
        ///  Gets the route invocation for a given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>

        RouteInvocation IFeature.GetRouteInvocation(VirtualPath path)
        {
            return _routes.GetRouteInvocation(path, this);
        }


        /// <summary>
        ///  Gets the container builder.
        /// </summary>

        IContainerBuilder IFeature.ContainerBuilder
        {
            get {
                return _containerConfiguration.Builder;
            }
        }

        #region /* ====== Old API, being replaced with the new fluent API ====== */


        /// <summary>
        ///  Gets or sets the <see cref="IModelBinder"/> instance used to invoke
        ///  actions provided by this feature.
        /// </summary>

        public IModelBinder ModelBinder { get; set; }


        /// <summary>
        ///  Creates a new instance of this feature.
        /// </summary>

        protected Feature()
        {
            this.ModelBinder = Dolstagis.Web.ModelBinding.ModelBinder.Default;
        }


        /// <summary>
        ///  Registers a <see cref="Controller"/> in this feature by type,
        ///  with a route specified in a [Route] attribute on the controller
        ///  class declaration.
        /// </summary>
        /// <typeparam name="T"></typeparam>

        public void AddController<T>()
        {
            var attributes = typeof(T).GetCustomAttributes(typeof(RouteAttribute), true);
            if (!attributes.Any()) {
                throw new ArgumentException("Type " + typeof(T) + " does not declare any routes, and no route was specified.");
            }

            foreach (RouteAttribute attr in attributes) {
                AddController<T>(attr.Route);
            }
        }

        /// <summary>
        ///  Registers a <see cref="Controller"/> in this feature with a specified route.
        /// </summary>
        /// <typeparam name="T">
        ///  The type of this controller.
        /// </typeparam>
        /// <param name="route">
        ///  The route definition for this controller.
        /// </param>

        public void AddController<T>(VirtualPath route)
        {
            this._routes.Add(route, new RouteTarget(typeof(T)));
        }

        /* ====== AddStaticFiles and AddViews helper methods ====== */

        protected void AddStaticResources(ResourceType type, VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory)
        {
            Container.Setup.Feature(c => {
                c.Add<ResourceMapping>(ioc => new ResourceMapping(type, path, locationFactory(ioc)), Scope.Request);
            });
       }

        protected void AddStaticResources(ResourceType type, VirtualPath path, IResourceLocation location)
        {
            Container.Setup.Feature(c => {
                c.Add<ResourceMapping>(new ResourceMapping(type, path, location));
            });
        }

        protected void AddStaticResources(ResourceType type, VirtualPath path, string physicalPath)
        {
            AddStaticResources(type, path, new FileResourceLocation(physicalPath));
            AddStaticFilesController(path);
        }

        private void AddStaticFilesController(VirtualPath path)
        {
            AddController<StaticRequestController>(path.Append("{path*}"));
        }

        /* ====== AddStaticFiles methods ====== */

        /// <summary>
        ///  Registers a directory or file of static files,
        ///  using a location created with dependencies taken from the IOC container.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="locationFactory"></param>

        public void AddStaticFiles(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory)
        {
            AddStaticResources(ResourceType.StaticFiles, path, locationFactory);
            AddStaticFilesController(path);
        }


        /// <summary>
        ///  Registers a directory or file of static files at the specified location.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="location"></param>

        public void AddStaticFiles(VirtualPath path, IResourceLocation location)
        {
            AddStaticResources(ResourceType.StaticFiles, path, location);
            AddStaticFilesController(path);
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
            AddStaticFilesController(path);
        }


        /* ====== AddViews methods ====== */

        /// <summary>
        ///  Registers a directory or file of views,
        ///  using a location created with dependencies taken from the IOC container.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="locationFactory"></param>

        public void AddViews(VirtualPath path, Func<IIoCContainer, IResourceLocation> locationFactory)
        {
            AddStaticResources(ResourceType.Views, path, locationFactory);
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

        #endregion
    }
}
