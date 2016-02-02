using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;
using Dolstagis.Web.Aspnet;

namespace $rootnamespace$
{
    public class MainFeature : Feature
    {
        public MainFeature()
        {
            this.AddController<Index>();
        }
    }
}
