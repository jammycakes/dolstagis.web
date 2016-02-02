using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace Dolstagis.Web.Aspnet
{
    public static class ApplicationExtensions
    {
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

        internal static IEnumerable<Assembly> FindAssemblies(this Application application)
        {
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
            return assemblies;
        }


        public static void ScanForFeatures(this Application application)
        {
            foreach (var assembly in FindAssemblies(application)) {
                application.AddAllFeaturesInAssembly(assembly);
            }
        }
    }
}
