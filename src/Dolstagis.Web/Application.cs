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
            _container.Configure(x => {
                x.For<Application>().Singleton().Use(this);
                x.For<RouteTable>().Singleton();
                x.For<IRequestProcessor>().Use<RequestProcessor>();
                x.For<IExceptionHandler>().Use<ExceptionHandler>();
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
            using (var childContainer = _container.GetNestedContainer()) {
                Exception fault = null;
                try {
                    var requestProcessor = childContainer.GetInstance<IRequestProcessor>();
                    await requestProcessor.ProcessRequest(context); 
                }
                catch (Exception ex) {
                    fault = ex;
                }

                if (fault != null) {
                    var exceptionHandler = childContainer.GetInstance<IExceptionHandler>();
                    await exceptionHandler.HandleException(context, fault);
                }
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
