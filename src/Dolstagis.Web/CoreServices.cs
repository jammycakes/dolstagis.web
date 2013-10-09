using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            For<IResourceLocator>().Singleton().Use<ResourceLocator>();

            For<IResultProcessor>().Singleton().Add<StaticResultProcessor>()
                .Ctor<IResourceLocator>().Is(ctx => new ResourceLocator
                    ("StaticFiles", ctx.GetAllInstances<ResourceLocation>())
                );

            For<IResultProcessor>().Singleton().Add<ViewResultProcessor>();

            For<ViewRegistry>().Singleton().Use<ViewRegistry>()
                .Ctor<IResourceLocator>().Is(ctx => new ResourceLocator
                    ("Views", ctx.GetAllInstances<ResourceLocation>())
                );
        }
    }
}
