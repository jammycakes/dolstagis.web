using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace Dolstagis.Web.FeatureSwitches
{
    /// <summary>
    ///  A feature switch whose on/off state is read from the appSettings section
    ///  of the web.config file.
    /// </summary>

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AppSettingsSwitchableAttribute : Attribute, IFeatureSwitchBuilder
    {
        public string AppSettingName { get; private set; }

        public bool EnableByDefault { get; private set; }


        /// <summary>
        ///  Creates a new instance of the <see cref="AppSettingsSwitchableAttribute"/> class.
        /// </summary>
        /// <param name="feature">
        ///  The feature to be controlled by this switch.
        /// </param>
        /// <param name="appSettingName">
        ///  The name of the setting in the appSettings section of the config file.
        ///  If not specified, defaults to the full name of the feature's type.
        /// </param>
        /// <param name="enableByDefault">
        ///  The default setting of this switch if no entry is found in the config file.
        /// </param>

        public AppSettingsSwitchableAttribute
            (string appSettingName = null, bool enableByDefault = true)
        {
            this.AppSettingName = appSettingName;
            this.EnableByDefault = enableByDefault;
        }

        public IFeatureSwitch CreateSwitch(Feature feature, IContainer container)
        {
            string key = AppSettingName ?? feature.GetType().FullName;
            string appSetting = ConfigurationManager.AppSettings[key];
            bool enabled;
            if (!bool.TryParse(appSetting, out enabled)) enabled = EnableByDefault;
            return new BasicSwitch(enabled);
        }
    }
}