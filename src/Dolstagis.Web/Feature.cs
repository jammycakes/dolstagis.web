﻿using System;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.IoC;
using Dolstagis.Web.IoC.DSL;
using Dolstagis.Web.IoC.Impl;
using Dolstagis.Web.Logging;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

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
        private int _priority = 0;
        private readonly FeatureSwitch _switch = new FeatureSwitch();
        private string _description = null;
        private IRouteTable _routes = new RouteTable();
        private readonly ContainerConfiguration _containerConfiguration
            = new ContainerConfiguration();
        private IModelBinder _modelBinder = ModelBinding.ModelBinder.Default;
        private ViewTable _views = new ViewTable();

        /// <summary>
        ///  Creates a new instance of this feature.
        /// </summary>

        protected Feature()
        {
            _containerConfiguration.SettingContainer += (s, e) =>
                _switch.AssertNotDefined(
                    "You can not specify an explicit IOC container in the feature " +
                    this.GetType().FullName + " as it is switchable. Please remove the feature " +
                    "switch or specify the container by type only. If you want to specify an " +
                    "explicit IOC container, please do so in a non-switchable feature."
                );
            _switch.Defining += (s, e) => {
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

        protected ISwitchExpression Activate
        {
            get {
                AssertConstructing();
                return _switch;
            }
        }


        /// <summary>
        ///  Configures the IOC containers.
        /// </summary>

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
        ///  Configures the model binder for controllers defined in this feature.
        /// </summary>
        /// <typeparam name="TModelBinder"></typeparam>

        protected void ModelBinder<TModelBinder>()
            where TModelBinder : IModelBinder, new()
        {
            AssertConstructing();
            _modelBinder = new TModelBinder();
        }


        /// <summary>
        ///  Configures the model binder for controllers defined in this feature.
        /// </summary>
        /// <param name="binder"></param>

        protected void ModelBinder(IModelBinder binder)
        {
            AssertConstructing();
            _modelBinder = binder;
        }


        /// <summary>
        ///  Defines the priority of this feature.
        /// </summary>
        /// <param name="priority"></param>
        /// <remarks>
        ///  Switchable features always take priority over non-switchable features.
        ///  Within the two groupings, this specifies the priority of the feature.
        /// </remarks>

        protected void Priority(int priority)
        {
            AssertConstructing();
            _priority = priority;
        }


        /// <summary>
        ///  Provides a fluent interface to configure routes to controllers.
        /// </summary>

        protected IRouteExpression Route
        {
            get {
                AssertConstructing();
                return new RouteExpression(_routes, this);
            }
        }

        /// <summary>
        ///  Provides a fluent interface to configure routes to views.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>

        protected IStaticFilesExpression Views(VirtualPath specification)
        {
            AssertConstructing();
            return new ViewExpression(_views, specification);
        }

        /* ====== Public properties and methods ====== */

        /*
         * The feature's state is only intended to be accessible from external
         * classes, specifically, the rest of the framework.
         * 
         * To avoid polluting IntelliSense with irrelevancies, these must be
         * declared as members of the IFeature interface, which must be
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
            _constructed = true;
            return _routes.GetRouteInvocation(path, this);
        }


        /// <summary>
        ///  Gets the container builder.
        /// </summary>

        IContainerBuilder IFeature.ContainerBuilder
        {
            get {
                _constructed = true;
                return _containerConfiguration.Builder;
            }
        }


        /// <summary>
        ///  Gets the <see cref="IModelBinder"/> instance used to invoke
        ///  actions provided by this feature.
        /// </summary>

        IModelBinder IFeature.ModelBinder {
            get {
                _constructed = true;
                return _modelBinder;
            }
        }

        /// <summary>
        ///  Gets the <see cref="ViewTable"/> instance used to look up views.
        /// </summary>

        ViewTable IFeature.Views
        {
            get {
                _constructed = true;
                return _views;
            }
        }

        int IFeature.Priority
        {
            get {
                _constructed = true;
                return _priority;
            }
        }
    }
}
