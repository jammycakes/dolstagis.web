using Dolstagis.Web.Auth;
using Dolstagis.Web.Http;
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

                c.Use<IRequest>(ctx => ctx.GetService<IRequestContext>().Request, Scope.Request);
                c.Use<IResponse>(ctx => ctx.GetService<IRequestContext>().Response, Scope.Request);
                c.Use<IUser>(ctx => ctx.GetService<IRequestContext>().User, Scope.Request);
                c.Use<ISession>(ctx => ctx.GetService<IRequestContext>().Session, Scope.Request);
            })
            .Setup.Request(c => {
            });
        }
    }
}
