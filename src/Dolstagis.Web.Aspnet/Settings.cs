using System.Configuration;
using System.Web.Configuration;

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
