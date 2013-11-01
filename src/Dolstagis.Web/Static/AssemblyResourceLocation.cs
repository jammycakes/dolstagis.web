using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class AssemblyResourceLocation : IResourceLocation
    {
        public Assembly Assembly { get; private set; }

        public string RootNamespace { get; private set; }

        public AssemblyResourceLocation(Assembly assembly, string rootNamespace)
        {
            this.Assembly = assembly;
            this.RootNamespace = RootNamespace;
        }


        public IResource GetResource(VirtualPath path)
        {
            var resourceName = RootNamespace + "." + String.Join(".", path.Parts.ToArray());
            var name = this.Assembly.GetManifestResourceNames().FirstOrDefault
                (x => x.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
            if (name == null) Status.NotFound.Throw();

            return new AssemblyResource(this.Assembly, name);
        }
    }
}
