using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheModule : Module
    {
        public NustacheModule()
        {
            this.Services.For<IViewEngine>().Singleton().Add<NustacheViewEngine>();
        }
    }
}
