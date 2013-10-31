using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class AssemblyResourceLocation : ResourceMapping
    {
        public Assembly Assembly { get; private set; }

        public string RootNamespace { get; private set; }

        public AssemblyResourceLocation(string type, VirtualPath root, Assembly assembly, string rootNamespace)
            : base(type, root)
        {
            this.Assembly = assembly;
            this.RootNamespace = RootNamespace;
        }


        protected override IResource CreateResource(VirtualPath path)
        {
            var resourceName = RootNamespace + "." + String.Join(".", path.Parts.ToArray());
            var name = this.Assembly.GetManifestResourceNames().FirstOrDefault
                (x => x.Equals(resourceName, StringComparison.OrdinalIgnoreCase));
            if (name == null) Status.NotFound.Throw();

            return new AssemblyResource(this.Assembly, name);
        }
    }
}
