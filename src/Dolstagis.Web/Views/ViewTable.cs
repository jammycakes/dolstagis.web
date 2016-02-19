using System;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Routes.Trie;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewTable : Trie<ViewNode, ViewRegistration>
    {
        public void Add(VirtualPath path, Func<VirtualPath, IServiceLocator, IResource> location)
        {
            if (path.Parts.LastOrDefault() != "*")
                path = path.Append("*");
            Add(path, new ViewRegistration() { Location = location });
        }
    }
}
