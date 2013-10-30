﻿using System;
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

    public class ApplicationContext : IDisposable, IApplicationContext
    {
        private IContainer _container;

        /// <summary>
        ///  Gets the root virtual path of the application. Does not include leading
        ///  or trailing slashes.
        /// </summary>

        public VirtualPath VirtualPath { get; private set; }

        /// <summary>
        ///  Gets the root physical path of the application.
        /// </summary>

        public string PhysicalPath { get; private set; }

        /// <summary>
        ///  Called by the application container (an HTTP application, for example)
        ///  to perform any setup tasks before requests can be processed.
        /// </summary>

        public ApplicationContext(string virtualPath, string physicalPath, IEnumerable<Module> modules)
        {
            this.VirtualPath = new VirtualPath(virtualPath);
            this.PhysicalPath = physicalPath;

            _container = new Container();
            foreach (var module in modules)
            {
                _container.Configure(x =>
                {
                    x.AddRegistry(module.Services);
                    x.For<Module>().Singleton().Add(module);
                    x.For<IRouteRegistry>().Singleton().Add(module);
                });
            }

            _container.Configure(x => {
                x.For<ApplicationContext>().Singleton().Use(this);
                x.For<IApplicationContext>().Singleton().Use(this);
                x.AddRegistry<CoreServices>();
            });
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
            var requestWrapper = new Request(request);
            var responseWrapper = new Response(response);

            using (var childContainer = _container.GetNestedContainer()) {
                childContainer.Configure(x => {
                    x.For<IRequest>().Use(requestWrapper);
                    x.For<IResponse>().Use(responseWrapper);
                    x.For<Request>().Use(requestWrapper);
                    x.For<Response>().Use(responseWrapper);
                });
                await childContainer.GetInstance<IRequestProcessor>()
                    .ProcessRequest(requestWrapper, responseWrapper);
            }
        }

        /// <summary>
        ///  Releases any resources when the application shuts down.
        /// </summary>

        public void Dispose()
        {
        }
    }
}
