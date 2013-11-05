using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web
{
    public class Application
    {
        private string _virtualPath;
        private string _physicalPath;
        private IList<Module> _modules = new List<Module>();
        private Lazy<ApplicationContext> _context;
        private ISettings _settings;
        private ISet<Assembly> _loadedAssemblies = new HashSet<Assembly>();

        public Application(string virtualPath, string physicalPath, ISettings settings)
        {
            _virtualPath = virtualPath;
            _physicalPath = physicalPath;
            _context = new Lazy<ApplicationContext>(CreateContext);
            _settings = settings;
            AddAllModulesInAssembly(this.GetType().Assembly);
        }

        private ApplicationContext CreateContext()
        {
            return new ApplicationContext(_virtualPath, _physicalPath, _settings, _modules.Where(x => x.Enabled));
        }


        /// <summary>
        ///  Registers a module with the application by type.
        /// </summary>
        /// <typeparam name="T">
        ///  They type of module to register.
        /// </typeparam>

        public void AddModule<T>() where T : Module, new()
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
            _modules.Add(module);
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
            if (_loadedAssemblies.Contains(assembly)) return;
            _loadedAssemblies.Add(assembly);

            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            foreach (var type in types.Where(t => typeof(Module).IsAssignableFrom(t)))
            {
                var constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
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
            await _context.Value.ProcessRequestAsync(request, response);
        }
    }
}
