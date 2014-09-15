using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.FeatureSwitches
{
    /// <summary>
    ///  A feature switch whose on/off state is read from the appSettings section
    ///  of the web.config file.
    /// </summary>

    public class AppSettingsFeatureSwitch : IFeatureSwitch
    {
        private readonly bool _enabled;

        public Feature Feature { get; private set; }


        public bool DependentOnRequest { get { return false; } }

        public Task<bool> IsEnabledForRequest(Http.IRequest request)
        {
            return Task.FromResult(_enabled);
        }

        /// <summary>
        ///  Creates a new instance of the <see cref="AppSettingsFeatureSwitch"/> class.
        /// </summary>
        /// <param name="feature">
        ///  The feature to be controlled by this switch.
        /// </param>
        /// <param name="appSettingName">
        ///  The name of the setting in the appSettings section of the config file.
        ///  If not specified, defaults to the full name of the feature's type.
        /// </param>
        /// <param name="defaultValue">
        ///  The default setting of this switch if no entry is found in the config file.
        /// </param>

        public AppSettingsFeatureSwitch
            (Feature feature, string appSettingName = null, bool defaultValue = true)
        {
            if (feature == null) throw new ArgumentNullException("feature");
            Feature = feature;
            appSettingName = appSettingName ?? feature.GetType().FullName;
            string appSetting = ConfigurationManager.AppSettings[appSettingName];
            if (!bool.TryParse(appSetting, out _enabled)) _enabled = defaultValue;
        }
    }
}
