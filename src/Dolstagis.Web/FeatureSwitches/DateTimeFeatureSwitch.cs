using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.FeatureSwitches
{
    /// <summary>
    ///  A feature switch which is automatically activated or deactivated at a
    ///  given date and time.
    /// </summary>

    public class DateTimeFeatureSwitch : IFeatureSwitch
    {
        public DateTimeFeatureSwitchType Type { get; private set; }

        public DateTime SwitchingTime { get; private set; }

        public Task<bool> IsEnabledForRequest(Http.IRequest request)
        {
            bool isPassed = DateTime.UtcNow >= SwitchingTime;
            bool isEnabled = isPassed ^ (Type == DateTimeFeatureSwitchType.Deactivate);
            return Task.FromResult(isEnabled);
        }

        public Feature Feature { get; private set; }


        /// <summary>
        ///  Creates a new instance of the <see cref="DateTimeFeatureSwitch" />
        ///  instance.
        /// </summary>
        /// <param name="feature">
        ///  The feature controlled by this switch.
        /// </param>
        /// <param name="switchingTime">
        ///  The time at which the feature is to be switched on or off.
        ///  Note that this is compared against DateTime.UtcNow.
        /// </param>
        /// <param name="type">
        ///  The type of switch: whether the feature is to be switched on or off.
        /// </param>

        public DateTimeFeatureSwitch(DateTime switchingTime,
            DateTimeFeatureSwitchType type = DateTimeFeatureSwitchType.Activate)
        {
            this.SwitchingTime = switchingTime;
            this.Type = type;
        }
    }
}
