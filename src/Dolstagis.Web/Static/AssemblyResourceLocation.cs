using System;
using System.Linq;
using System.Reflection;

namespace Dolstagis.Web.Static
{
    public class AssemblyResourceLocation : IResourceLocation
    {
        public Assembly Assembly { get; private set; }

        public string RootNamespace { get; private set; }

        public AssemblyResourceLocation(Assembly assembly, string rootNamespace)
        {
            if (rootNamespace == null) throw new ArgumentNullException("rootNamespace");

            this.Assembly = assembly;
            this.RootNamespace = rootNamespace;
        }


        public IResource GetResource(VirtualPath path)
        {
            var resourceName = String.Join(".",
                new[] { RootNamespace }.Concat(path.Parts)
            );
            var name = this.Assembly.GetManifestResourceNames().FirstOrDefault
                (x => x.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
            if (name == null) return null;

            return new AssemblyResource(this.Assembly, name);
        }
    }
}
