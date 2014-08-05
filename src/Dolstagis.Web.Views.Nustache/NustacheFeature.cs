using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheFeature : Feature
    {
        public NustacheFeature()
        {
            this.Services.For<IViewEngine>().Singleton().Add<NustacheViewEngine>();
        }
    }
}
