using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewRegistration
    {
        public Func<VirtualPath, IServiceLocator, IResource> Location { get; set; }
    }
}
