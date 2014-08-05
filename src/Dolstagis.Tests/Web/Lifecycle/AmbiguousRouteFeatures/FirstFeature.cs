using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.Lifecycle.AmbiguousRouteFeatures
{
    public class FirstFeature : Feature
    {
        public FirstFeature()
        {
            this.AddHandler<FirstHandler>("HandledByFirst");
            this.AddHandler<NullHandler>("HandledBySecond");
            this.AddHandler<ArgHandler>("args/{arg}");
            this.AddHandler<FirstHandler>("args/the-first");
        }
    }
}
