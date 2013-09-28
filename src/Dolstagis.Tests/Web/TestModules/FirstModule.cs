using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Tests.Web.TestModules.Handlers;
using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestModules
{
    public class FirstModule : Module
    {
        public FirstModule()
        {
            AddHandler<RootHandler>();
            AddHandler<ChildHandler>("one/two");
            AddHandler<ChildHandler>("one/three");
            AddHandler<ChildHandler>("one/two/three/{id}");
            AddHandler<LanguageHandler>("{language}/one/two");
        }
    }
}
