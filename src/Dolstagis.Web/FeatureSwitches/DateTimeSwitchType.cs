using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.FeatureSwitches
{
    public enum DateTimeSwitchType
    {
        /// <summary>
        ///  The feature switch will be turned on at the specified date.
        /// </summary>
        Activate,

        /// <summary>
        ///  The feature switch will be turned off at the specified date.
        /// </summary>
        Deactivate
    }
}
