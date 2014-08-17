using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Dolstagis.Web.Aspnet
{
    public class HttpRequestHandler : HttpTaskAsyncHandler
    {
        private static Application _application;

        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public override async Task ProcessRequestAsync(System.Web.HttpContext context)
        {
            EnsureInit();
            var abstractContext = new HttpContextWrapper(context);

            var request = new Dolstagis.Web.Aspnet.HttpRequest(abstractContext.Request);
            var response = new Dolstagis.Web.Aspnet.HttpResponse(abstractContext.Response);
            await _application.ProcessRequestAsync(request, response);
        }

        static readonly string[] _ignores = new string[] {
            "Microsoft.",
            "mscorlib,",
            "System.",
            "System,",
            "IronPython",
            "IronRuby",
            "CR_ExtUnitTest",
            "CR_VSTest",
            "DevExpress.CodeRush",
            "StructureMap,",
            "Nustache.Core,",
            "Newtonsoft.Json,",
            "Dolstagis.Web,"
        };

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void EnsureInit()
        {
            if (_application != null) return;
            _application = new Application
                (HostingEnvironment.ApplicationVirtualPath, new Settings());

            var assemblies =
                from assembly in BuildManager.GetReferencedAssemblies().Cast<Assembly>()
                where !assembly.IsDynamic
                where !_ignores.Any(x => assembly.FullName.StartsWith(x, StringComparison.InvariantCulture))
                let order =
                    assembly.FullName.StartsWith("Dolstagis.Web,", StringComparison.InvariantCulture) ? 0 :
                    assembly.FullName.StartsWith("Dolstagis.Web.") ? 1 :
                    2
                orderby order
                select assembly;

            foreach (var assembly in assemblies) {
                _application.AddAllFeaturesInAssembly(assembly);
            }
        }
    }
}
