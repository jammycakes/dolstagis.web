using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web._dolstagis
{
    internal class DolstagisFeature : Feature
    {

        public DolstagisFeature()
        {
            string ns = this.GetType().Namespace;
            this.AddStaticFiles("~/_dolstagis/content",
                new AssemblyResourceLocation(this.GetType().Assembly, ns + ".content"));
            this.AddStaticResources(ResourceType.StaticFiles, new VirtualPath("~/_dolstagis/index.html"),
                new AssemblyResourceLocation(this.GetType().Assembly, ns + ".index.html"));
        }
    }
}
