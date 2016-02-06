using System;
using System.Linq.Expressions;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features.Impl
{
    public class RouteExpression : IRouteExpression, IRouteDestinationExpression,
        IStaticFilesExpression
    {
        private RouteTable _routes;
        private VirtualPath _path;

        public RouteExpression(RouteTable routes, VirtualPath path)
        {
            _routes = routes;
            _path = path;
        }

        IRouteDestinationExpression IRouteExpression.To
        {
            get {
                return this;
            }
        }

        IControllerExpression IRouteDestinationExpression.Controller
            (Expression<Func<IServiceProvider, object>> controllerFunc)
        {
            var target = new RouteTarget(controllerFunc);
            _routes.Add(_path, target);
            return target;
        }

        IStaticFilesExpression IRouteDestinationExpression.StaticFiles
        {
            get {
                return this;
            }
        }

        void IStaticFilesExpression.FromResource(Func<VirtualPath, IServiceProvider, IResource> locator)
        {
            var target = new RouteTarget(svc => new StaticFileController(svc, locator));
            _routes.Add(_path.Append("{path*}"), target);
        }
    }
}
