using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Aspnet;

namespace $rootnamespace$
{
    public class Configuration : IConfigurator
    {
        public void Configure(Dolstagis.Web.Application application)
        {
            application.ScanForFeatures();
        }
    }
}
