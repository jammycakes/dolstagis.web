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

        public ApplicationContext(string virtualPath, string physicalPath)
        {
            this.VirtualPath = new VirtualPath(virtualPath);
            this.PhysicalPath = physicalPath;

            _container = new Container();
            _container.Configure(x => {
                x.For<ApplicationContext>().Singleton().Use(this);
                x.For<IApplicationContext>().Singleton().Use(this);
                x.AddRegistry<CoreServices>();
            });
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
                x.For<IRouteRegistry>().Singleton().Add(module);
            });
        }

        /// <summary>
        ///  Scan an assembly for modules to add.
        /// </summary>
        /// <param name="assembly">
        ///  The assembly.
        /// </param>
        /// <remarks>
        ///  Only modules with a public default constructor will be instantiated.
        ///  The order in which they are added is non-deterministic.
        /// </remarks>

        public void AddAllModulesInAssembly(Assembly assembly)
        {
            Type[] types;

            try {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex) {
                types = ex.Types;
            }

            foreach (var type in types.Where(t => typeof(Module).IsAssignableFrom(t)))
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null) {
                    var module = constructor.Invoke(null) as Module;
                    AddModule(module);
                }
            }
        }

        /// <summary>
        ///  Processes a request synchronously.
        /// </summary>
        /// <param name="context">
        ///  The <see cref="IRequestContext"/> containing request and response objects.
        /// </param>

        public void ProcessRequest(IRequest request, IResponse response)
        {
            ProcessRequestAsync(request, response).Wait();
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