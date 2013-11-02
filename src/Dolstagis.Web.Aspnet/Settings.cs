using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dolstagis.Web.Aspnet
{
    public class Settings : ISettings
    {
        private bool _debug;

        public bool Debug
        {
            get { return _debug; }
        }

        public Settings()
        {
            var configSection = (CompilationSection)
                ConfigurationManager.GetSection("system.web/compilation");
            _debug = configSection != null && configSection.Debug;
        }
    }
}
