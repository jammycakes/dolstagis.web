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

        /// <summary>
        ///  Creates a new instance of this feature.
        /// </summary>

        protected Feature()
        {
            this.ModelBinder = Dolstagis.Web.ModelBinding.ModelBinder.Default;
            _containerConfiguration.ConfiguringApplication += (s, e) =>
                _switch.AssertNotDefined(
                    "You can not register services in the IOC container at Application level in " +
                    "the feature " + this.GetType().FullName + " as it is switchable. Please " +
                    "remove the feature switch or configure the services at Feature or Request " +
                    "level, or in a separate, non-switchable feature.");
            _containerConfiguration.SettingContainer += (s, e) =>
                _switch.AssertNotDefined(
                    "You can not specify an explicit IOC container in the feature " +
                    this.GetType().FullName + " as it is switchable. Please remove the feature " +
                    "switch or specify the container by type only. If you want to specify an " +
                    "explicit IOC container, please do so in a non-switchable feature."
                );
            _switch.Defining += (s, e) => {
                _containerConfiguration.AssertApplicationNotConfigured(
                    "You can not set a feature switch on the feature " + this.GetType().FullName +
                    " as it has registered services in the IOC container at Application level. " +
                    "Please remove the feature switch or configure the services at Feature or " +
                    "Request level, or in a separate, non-switchable feature."
                );
                _containerConfiguration.AssertContainerNotSet(
                    "You can not set a feature switch on the feature " + this.GetType().FullName +
                    " as it has specified an explicit IOC container. Please remove the feature " +
                    "switch or specify the container type only. If you want to specify an " +
                    "explicit IOC container, please do so in a non-switchable feature."
                );
            };
        }


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
            return new RouteExpression(_routes, specification);
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
