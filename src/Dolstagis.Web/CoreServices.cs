using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;
using StructureMap.Configuration.DSL;

namespace Dolstagis.Web
{
    internal class CoreServices : Registry
    {
        public CoreServices()
        {
            For<RouteTable>().Singleton().Use<RouteTable>();
            For<IMimeTypes>().Singleton().Use<MimeTypes>();

            For<IRequestContextBuilder>().Use<RequestContextBuilder>();
            For<IRequestProcessor>().Use<RequestProcessor>();
            For<IExceptionHandler>().Use<ExceptionHandler>();

            For<ISessionCookieBuilder>().Singleton().Use<SessionCookieBuilder>();

            For<IResultProcessor>().Singleton().Add<StaticResultProcessor>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    (ResourceType.StaticFiles, ctx.GetAllInstances<ResourceMapping>())
                );
            For<IResultProcessor>().Singleton().Add<ViewResultProcessor>();
            For<IResultProcessor>().Singleton().Add<JsonResultProcessor>();
            For<IResultProcessor>().Singleton().Add<ContentResultProcessor>();

            For<ViewRegistry>().Singleton().Use<ViewRegistry>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    (ResourceType.Views, ctx.GetAllInstances<ResourceMapping>())
                );
        }
    }
}
