using System;
using System.Linq;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.ModelBinding;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;

namespace Dolstagis.Web
{
    internal class CoreServices : Registry
    {
        public CoreServices()
        {
            For<IRequestProcessor>().Use<RequestProcessor>();
            For<IExceptionHandler>().Use<ExceptionHandler>();
            For<ISessionStore>().Singleton().Use<InMemorySessionStore>();
            For<IAuthenticator>().Singleton().Use<SessionAuthenticator>();
            For<ILoginHandler>().Use<LoginHandler>();

            For<IResultProcessor>().AlwaysUnique().Add<StaticResultProcessor>();
            For<IResultProcessor>().AlwaysUnique().Add<ViewResultProcessor>();
            For<IResultProcessor>().Add(JsonResultProcessor.Instance);
            For<IResultProcessor>().Add(ContentResultProcessor.Instance);
            For<IResultProcessor>().Add(HeadResultProcessor.Instance);

            For<ViewRegistry>().Use<ViewRegistry>();
        }
    }
}
