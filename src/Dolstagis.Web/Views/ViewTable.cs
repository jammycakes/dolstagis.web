using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewTable
    {
        private IList<ViewLocation> _locations = new List<ViewLocation>();

        public void Add(VirtualPath root, Func<VirtualPath, IServiceLocator, IResource> locator)
        {
            _locations.Add(new ViewLocation {
                Root = root,
                Locator = locator
            });
        }



        private class ViewLocation
        {
            public VirtualPath Root { get; set; }

            public Func<VirtualPath, IServiceLocator, IResource> Locator { get; set; }
        }
    }
}
