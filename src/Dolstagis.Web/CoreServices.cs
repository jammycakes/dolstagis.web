using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
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

            For<IHttpContextBuilder>().Use<HttpContextBuilder>();
            For<IRequestProcessor>().Use<RequestProcessor>();
            For<IExceptionHandler>().Use<ExceptionHandler>();
            For<ISessionStore>().Use(ctx => null);
            For<IAuthenticator>().Singleton().Use<PrincipalAuthenticator>();
            For<ILoginHandler>().Use<LoginHandler>();

            For<IResultProcessor>().Singleton().Add<StaticResultProcessor>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    (ResourceType.StaticFiles, ctx.GetAllInstances<ResourceMapping>())
                );
            For<IResultProcessor>().Singleton().Add<ViewResultProcessor>();
            For<IResultProcessor>().Singleton().Add<JsonResultProcessor>();
            For<IResultProcessor>().Singleton().Add<ContentResultProcessor>();
            For<IResultProcessor>().Singleton().Add<HeadResultProcessor>();

            For<ViewRegistry>().Singleton().Use<ViewRegistry>()
                .Ctor<IResourceResolver>().Is(ctx => new ResourceResolver
                    (ResourceType.Views, ctx.GetAllInstances<ResourceMapping>())
                );
        }
    }
}
