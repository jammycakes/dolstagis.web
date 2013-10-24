using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Routing;
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

            For<IResourceResolver>().Singleton().Use<ResourceResolver>();
            For<ISessionCookieBuilder>().Singleton().Use<SessionCookieBuilder>();

            For<IResultProcessor>().Singleton().Add<StaticResultProcessor>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    ("StaticFiles", ctx.GetAllInstances<ResourceLocation>())
                );

            For<IResultProcessor>().Singleton().Add<ViewResultProcessor>();
            For<IResultProcessor>().Singleton().Add<JsonResultProcessor>();

            For<ViewRegistry>().Singleton().Use<ViewRegistry>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    ("Views", ctx.GetAllInstances<ResourceLocation>())
                );
        }
    }
}
