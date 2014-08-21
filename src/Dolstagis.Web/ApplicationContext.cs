using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
using StructureMap;

namespace Dolstagis.Web
{
    /// <summary>
    ///  The Application object is the singleton instance at the root of our
    ///  application which marshals services and manages request lifecycles.
    /// </summary>

    public class ApplicationContext : IDisposable
    {
        private IContainer _container;

        /// <summary>
        ///  Called by the application container (an HTTP application, for example)
        ///  to perform any setup tasks before requests can be processed.
        /// </summary>

        public ApplicationContext(Application application, IEnumerable<Feature> features)
        {
            _container = new Container();
            _container.Configure(x =>
            {
                x.For<ISettings>().Singleton().Use(application.Settings);
                x.For<ApplicationContext>().Singleton().Use(this);
                x.AddRegistry<CoreServices>();
            });

            foreach (var feature in features)
            {
                _container.Configure(x =>
                {
                    x.AddRegistry(feature.Services);
                    x.For<Feature>().Singleton().Add(feature);
                    x.For<IRouteRegistry>().Singleton().Add(feature);
                });
            }
        }


        /// <summary>
        ///  Processes a request asynchronously.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="IHttpContext"/> containing request and response objects.
        /// </param>
        /// <returns>
        ///  A <see cref="Task"/> instance.
        /// </returns>

        public async Task ProcessRequestAsync(IRequest request, IResponse response)
        {
            using (var childContainer = _container.GetNestedContainer()) {
                childContainer.Configure(x => {
                    x.For<IRequest>().Use(request);
                    x.For<IResponse>().Use(response);
                });
                await childContainer.GetInstance<IRequestProcessor>()
                    .ProcessRequest(request, response);
            }
        }

        /// <summary>
        ///  Releases any resources when the application shuts down.
        /// </summary>

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
                _container = null;
            }
        }
    }
}
