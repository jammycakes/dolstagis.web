using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dolstagis.Web.Features;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Logging;
using Dolstagis.Web.Owin;
using Dolstagis.Web.Util;

namespace Dolstagis.Web
{
    public class Application
    {
        private static readonly Logger log = Logger.ForThisClass();

        /// <summary>
        ///  Gets the application-level IOC container.
        /// </summary>

        public IIoCContainer Container { get; private set; }

        /// <summary>
        ///  Gets the features which are available to the application.
        /// </summary>

        public FeatureSwitchboard Features { get; private set; }

        public ISettings Settings { get; private set; }
        
        private ISet<Assembly> _loadedAssemblies = new HashSet<Assembly>();

        public Application(ISettings settings)
        {
            log.Debug("Starting up Dolstagis.Web");
            log.Trace(() => "Settings: " + Newtonsoft.Json.JsonConvert.SerializeObject(settings));

            Features = new FeatureSwitchboard(this);
            Settings = settings;

            AddAllFeaturesInAssembly(this.GetType().Assembly);
        }


        /// <summary>
        ///  Registers a feature with the application by type.
        /// </summary>
        /// <typeparam name="T">
        ///  They type of feature to register.
        /// </typeparam>

        public void AddFeature<T>() where T : IFeature, new()
        {
            AddFeature(new T());
        }

        /// <summary>
        ///  Registers a feature with the application by instance.
        /// </summary>
        /// <param name="feature">
        ///  The feature to register.
        /// </param>

        public void AddFeature(IFeature feature)
        {
            log.Debug(() => "Found feature: " + feature.GetType().FullName);
            Features.Add(feature);
        }

        /// <summary>
        ///  Scan an assembly for features to add.
        /// </summary>
        /// <param name="assembly">
        ///  The assembly.
        /// </param>
        /// <remarks>
        ///  Only features with a public default constructor will be instantiated.
        ///  The order in which they are added is non-deterministic.
        /// </remarks>

        public void AddAllFeaturesInAssembly(Assembly assembly)
        {
            if (_loadedAssemblies.Contains(assembly)) return;
            _loadedAssemblies.Add(assembly);

            var features = assembly.SafeGetInstances<IFeature>();
            foreach (var feature in features)
                AddFeature(feature);
        }


        /* ====== Setup after loading features====== */

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Configure()
        {
            if (Container != null) return;

            // First find all features that specify an IOC container instance
            // They should all be the same instance, otherwise throw.

            const string iocError =
                "Conflicting IOC container declarations found. Please ensure " +
                "that all features declare IOC containers of the same type, and " +
                "that at most one specific container instance is registered " +
                "with Using<TContainer>().";

            const string iocMissingError =
                "No feature has defined an IOC container. Please make sure that " +
                "at least one feature has specified an IOC container using " +
                "Container.Is<TContainer>() in the constructor.";

            IIoCContainer instance = null;
            IContainerBuilder builder = null;

            foreach (var feature in Features) {
                var cb = feature.ContainerBuilder;
                if (cb.HasInstance) {
                    instance = instance ?? cb.Instance;
                    if (!Object.ReferenceEquals(instance, cb.Instance)) {
                        throw new InvalidOperationException(iocError);
                    }
                }
                else if (!cb.ContainerType.IsInterface) {
                    builder = builder ?? cb;
                    if (builder.ContainerType != cb.ContainerType) {
                        throw new InvalidOperationException(iocError);
                    }
                }
            }

            // Is there an instance? If so, check it and return it

            if (instance != null) {
                if (builder != null && !builder.ContainerType.IsInstanceOfType(instance)) {
                    throw new InvalidOperationException(iocError);
                }
            }
            else {
                if (builder == null) {
                    throw new InvalidOperationException(iocMissingError);
                }
                else {
                    instance = builder.CreateContainer();
                }
            }

            // Finally, configure the container from all features.

            instance.Use<ISettings>(Settings);
            instance.Use<Application>(this);

            foreach (var feature in Features) {
                feature.ContainerBuilder.SetupApplication(instance);
            }

            Container = instance;
        }


        /* ====== Request processing ====== */

        /// <summary>
        ///  Processes a request asynchronously.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="IRequestContext"/> containing request and response objects.
        /// </param>
        /// <returns>
        ///  A <see cref="Task"/> instance.
        /// </returns>

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            log.Debug(() =>
                request.Protocol + " " + request.Method + " " + request.AbsolutePath.ToString()
            );
            var featureSet = Features.GetFeatureSet(request);
            await featureSet.ProcessRequestAsync(request, response);
        }

        /// <summary>
        ///  Gets the Owin AppFunc for this request.
        /// </summary>
        /// <returns></returns>

        public Func<IDictionary<string, object>, Task> GetAppFunc()
        {
            return async environment => {
                Configure();
                var request = new Request(environment);
                var response = new Response(environment);
                await ProcessRequestAsync(request, response);
            };
        }
    }
}
