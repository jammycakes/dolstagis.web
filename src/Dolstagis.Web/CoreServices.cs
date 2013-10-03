using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Views.Static;
using StructureMap.Configuration.DSL;

namespace Dolstagis.Web
{
    internal class CoreServices : Registry
    {
        public CoreServices()
        {
            For<RouteTable>().Singleton().Use<RouteTable>();
            For<IMimeTypes>().Singleton().Use<MimeTypes>();

            For<IRequestContextBuilder>().Singleton().Use<RequestContextBuilder>();
            For<IRequestProcessor>().Use<RequestProcessor>();
            For<IExceptionHandler>().Use<ExceptionHandler>();

            For<IResultProcessor>().Singleton().Add<StaticResultProcessor>();
        }
    }
}
