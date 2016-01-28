using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Dolstagis.Web.Features.Impl;
using Dolstagis.Web.Http;
using Dolstagis.Web.Logging;
using Dolstagis.Web.Owin;
using Dolstagis.Web.Util;
using StructureMap;

namespace Dolstagis.Web
{
    public class Application
    {
        private static readonly Logger log = Logger.ForThisClass();

        /// <summary>
        ///  Gets the application-level IOC container.
        /// </summary>

        public IContainer Container { get; private set; }

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
            Container = new Container();
            Container.Configure(x => {
                x.For<ISettings>().Use(settings);
                x.For<Application>().Use(this);
                x.AddRegistry<CoreServices>();
            });

            AddAllFeaturesInAssembly(this.GetType().Assembly);
        }


        /// <summary>
        ///  Registers a feature with the application by type.
        /// </summary>
        /// <typeparam name="T">
        ///  They type of feature to register.
        /// </typeparam>

        public void AddFeature<T>() where T : Feature, new()
        {
            AddFeature(new T());
        }

        /// <summary>
        ///  Registers a feature with the application by instance.
        /// </summary>
        /// <param name="feature">
        ///  The feature to register.
        /// </param>

        public void AddFeature(Feature feature)
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

            var features = assembly.SafeGetInstances<Feature>();
            foreach (var feature in features)
                AddFeature(feature);
        }


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
                var request = new Request(environment);
                var response = new Response(environment);
                await ProcessRequestAsync(request, response);
            };
        }
    }
}
