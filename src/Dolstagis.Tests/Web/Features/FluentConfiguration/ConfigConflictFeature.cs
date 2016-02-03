using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Features.FluentConfiguration
{
    public class ConfigConflictFeature : Feature
    {
        public ConfigConflictFeature(bool createSwitchFirst, bool mapGlobal, bool createSwitchSecond)
        {
            if (createSwitchFirst) {
                Active.When(() => false);
            }

            if (mapGlobal) {
                Container.Setup.Application(c => { });
            }

            if (createSwitchSecond) {
                Active.When(() => false);
            }
        }
    }
}
