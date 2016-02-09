using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Auth;
using Dolstagis.Web.Features;
using Dolstagis.Web.Http;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Lifecycle;
using Dolstagis.Web.Lifecycle.ResultProcessors;
using Dolstagis.Web.Sessions;
using Dolstagis.Web.Static;
using Dolstagis.Web.Views;

namespace Dolstagis.Web.Lifecycle
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

                c.Add<IResultProcessor, ResourceResultProcessor>(Scope.Transient);
                c.Add<IResultProcessor, ViewResultProcessor>(Scope.Transient);
                c.Add<IResultProcessor>(JsonResultProcessor.Instance);
                c.Add<IResultProcessor>(ContentResultProcessor.Instance);
                c.Add<IResultProcessor>(HeadResultProcessor.Instance);

                c.Use<ViewRegistry, ViewRegistry>(Scope.Request);

                c.Use<IRequestContext, NullRequestContext>(Scope.Transient);
                c.Use<IRequest>(ctx => ctx.GetService<IRequestContext>().Request, Scope.Request);
                c.Use<IResponse>(ctx => ctx.GetService<IRequestContext>().Response, Scope.Request);
                c.Use<IUser>(ctx => ctx.GetService<IRequestContext>().User, Scope.Request);
                c.Use<ISession>(ctx => ctx.GetService<IRequestContext>().Session, Scope.Request);
            })
            .Setup.Request(c => {
            });

            Route("~/").To.StaticFiles.FromAssemblyResources
                (this.GetType().Assembly, "Dolstagis.Web._dolstagis.index.html");
            Route("~/_dolstagis").To.StaticFiles.FromAssemblyResources
                (this.GetType().Assembly, "Dolstagis.Web._dolstagis");

        }


        /* ====== NullRequestContext ====== */

        /*
         * The IRequestContext interface is only intended to be used within the
         * scope of an HTTP request, and therefore should only be instantiated
         * by the request container. However, it needs to be configured at the
         * application container level, since some IOC containers (i.e.
         * StructureMap) don't support configuring lambda expressions in the
         * request-level container, which is necessary to allow for lazy
         * retrieval of the IUser and ISession instances (possibly from a
         * database).
         *
         * To get round this, we'll define a NullRequestContext to stick into
         * the top level container. This allows us to validate the container
         * configuration in containers that support doing so.
         */

        private class NullRequestContext : IRequestContext, IRequest, IResponse, IUser, ISession
        {
            private Exception GetException()
            {
                throw new NotImplementedException
                    ("You have attempted to use an IRequestContext that was instantiated outside of a request. " +
                    "This is not supported because IRequestContext is only meaingful within a request.");
            }

            public VirtualPath AbsolutePath
            {
                get {
                    throw GetException();
                }
            }

            public Stream Body
            {
                get {
                    throw GetException();
                }
            }

            public IServiceProvider Container
            {
                get {
                    throw GetException();
                }
            }

            public DateTime? Expires
            {
                get {
                    throw GetException();
                }
            }

            public IDictionary<string, string[]> Form
            {
                get {
                    throw GetException();
                }
            }

            public RequestHeaders Headers
            {
                get {
                    throw GetException();
                }
            }

            public string ID
            {
                get {
                    throw GetException();
                }
            }

            public bool IsSecure
            {
                get {
                    throw GetException();
                }
            }

            public IDictionary<string, object> Items
            {
                get {
                    throw GetException();
                }
            }

            public string Method
            {
                get {
                    throw GetException();
                }
            }

            public VirtualPath Path
            {
                get {
                    throw GetException();
                }
            }

            public VirtualPath PathBase
            {
                get {
                    throw GetException();
                }
            }

            public string Protocol
            {
                get {
                    throw GetException();
                }
            }

            public IDictionary<string, string[]> Query
            {
                get {
                    throw GetException();
                }
            }

            public IRequest Request
            {
                get {
                    return this;
                }
            }

            public IResponse Response
            {
                get {
                    return this;
                }
            }

            public ISession Session
            {
                get {
                    return this;
                }
            }

            public Status Status
            {
                get {
                    throw GetException();
                }

                set {
                    throw GetException();
                }
            }

            public Uri Url
            {
                get {
                    throw GetException();
                }
            }

            public IUser User
            {
                get {
                    return this;
                }
            }

            public string UserName
            {
                get {
                    throw GetException();
                }
            }

            ResponseHeaders IResponse.Headers
            {
                get {
                    throw GetException();
                }
            }

            string IResponse.Protocol
            {
                get {
                    throw GetException();
                }

                set {
                    throw GetException();
                }
            }

            public void AddHeader(string name, string value)
            {
                throw GetException();
            }

            public void End()
            {
                throw GetException();
            }

            public Task<object> GetItemAsync(string key)
            {
                throw GetException();
            }

            public Task<ISession> GetSessionAsync()
            {
                throw GetException();
            }

            public Task<IUser> GetUserAsync()
            {
                throw GetException();
            }

            public bool IsInRole(string role)
            {
                throw GetException();
            }

            public Task Persist()
            {
                throw GetException();
            }

            public Task SetItemAsync(string key, object value)
            {
                throw GetException();
            }
        }
    }
}
