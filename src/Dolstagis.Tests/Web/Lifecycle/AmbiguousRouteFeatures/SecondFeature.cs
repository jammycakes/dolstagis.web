using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteFeatures
{
    public class SecondFeature : Feature
    {
        public SecondFeature()
        {
            this.AddHandler<NullHandler>("HandledByFirst");
            this.AddHandler<SecondHandler>("HandledBySecond");
        }
    }
}
