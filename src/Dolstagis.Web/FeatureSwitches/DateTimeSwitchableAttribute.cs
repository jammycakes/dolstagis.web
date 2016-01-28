using System;
using System.Globalization;

namespace Dolstagis.Web.FeatureSwitches
{
    /// <summary>
    ///  A feature switch which is automatically activated or deactivated at a
    ///  given date and time.
    /// </summary>

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DateTimeSwitchableAttribute : Attribute, IFeatureSwitch
    {
        public DateTimeSwitchType Type { get; private set; }

        public DateTime SwitchingTime { get; private set; }

        public bool IsEnabledForRequest(Http.IRequest request)
        {
            bool isPassed = DateTime.UtcNow >= SwitchingTime;
            return isPassed ^ (Type == DateTimeSwitchType.Deactivate);
        }

        public Feature Feature { get; private set; }


        /// <summary>
        ///  Creates a new instance of the <see cref="DateTimeSwitchableAttribute" />
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

        public DateTimeSwitchableAttribute(DateTime switchingTime,
            DateTimeSwitchType type = DateTimeSwitchType.Activate)
        {
            this.SwitchingTime = switchingTime;
            this.Type = type;
        }

        public DateTimeSwitchableAttribute(string switchingTime,
            DateTimeSwitchType type = DateTimeSwitchType.Activate)
        {
            this.SwitchingTime = ParseDateTime(switchingTime);
            this.Type = type;
        }

        private static DateTime ParseDateTime(string dateString)
        {
            return DateTime.ParseExact(dateString,
                new string[] { "o", "r", "s", "u" },
                System.Globalization.CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal
            );
        }

    }
}
