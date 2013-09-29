﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Routing;
using StructureMap;

namespace Dolstagis.Web
{
    /// <summary>
    ///  The Application object is the singleton instance at the root of our
    ///  application which marshals services and manages request lifecycles.
    /// </summary>

    public class Application : IDisposable
    {
        private static IContainer _container;

        /// <summary>
        ///  Called by the application container (an HTTP application, for example)
        ///  to perform any setup tasks before requests can be processed.
        /// </summary>

        public void Init()
        {
            _container = ObjectFactory.Container;
                x.For<Application>().Singleton().Use(this);
                x.For<RouteTable>().Singleton();
        }

        /// <summary>
        ///  Registers a module with the application by type.
        /// </summary>
        /// <typeparam name="T">
        ///  They type of module to register.
        /// </typeparam>

        public void AddModule<T>() where T: Module, new()
        {
            AddModule(new T());
        }

        /// <summary>
        ///  Registers a module with the application by instance.
        /// </summary>
        /// <param name="module">
        ///  The module to register.
        /// </param>

        public void AddModule(Module module)
        {
            _container.Configure(x => {
                x.AddRegistry(module.Services);
                x.For<Module>().Singleton().Add(module);
            });
        }

        /// <summary>
        ///  Processes a request synchronously.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="IHttpContext"/> containing request and response objects.
        /// </param>

        public void ProcessRequest(IHttpContext context)
        {
            ProcessRequestAsync(context).Wait();
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

        public async Task ProcessRequestAsync(IHttpContext context)
        {
        }

        /// <summary>
        ///  Releases any resources when the application shuts down.
        /// </summary>

        public void Dispose()
        {
        }
    }
}
