using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web
{
    public class Module
    {
        public virtual string Description { get { return String.Empty; } }

        public bool Enabled { get; set; }

        public Module()
        {
            this.Enabled = true;
        }

        public void AddHandler<T>() where T: Handler
        {
        }

        public void AddHandler<T>(string route) where T: Handler
        {
        }

        public void AddHandler<T>(string route, Func<RouteInfo, bool> precondition) where T: Handler
        {
        }
    }
}
