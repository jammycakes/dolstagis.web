using Dolstagis.Web.Auth;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Lifecycle.ResultProcessors;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web
{
    internal class CoreServices : Feature
    {
        public CoreServices()
        {
            Container.Setup.Application(c => {
                c.Use<IExceptionHandler, ExceptionHandler>(Scope.Request);
                c.Use<ISessionStore, InMemorySessionStore>(Scope.Application);
                c.Use<IAuthenticator, SessionAuthenticator>(Scope.Application);
                c.Use<ILoginHandler, LoginHandler>(Scope.Request);

                c.Add<IResultProcessor, StaticResultProcessor>(Scope.Transient);
                c.Add<IResultProcessor, ViewResultProcessor>(Scope.Transient);
                c.Add<IResultProcessor>(JsonResultProcessor.Instance);
                c.Add<IResultProcessor>(ContentResultProcessor.Instance);
                c.Add<IResultProcessor>(HeadResultProcessor.Instance);

                c.Use<ViewRegistry, ViewRegistry>(Scope.Request);
            });
        }
    }
}
