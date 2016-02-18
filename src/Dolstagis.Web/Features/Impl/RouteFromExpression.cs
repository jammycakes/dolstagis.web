using System;
using System.Linq.Expressions;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Routes;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Features.Impl
{
    public class RouteFromExpression : IRouteFromExpression, IRouteDestinationExpression,
        IStaticFilesExpression
    {
        private IRouteTable _routes;
        private VirtualPath _path;

        public RouteFromExpression(IRouteTable routes, VirtualPath path)
        {
            _routes = routes;
            _path = path;
        }

        IRouteDestinationExpression IRouteFromExpression.To
        {
            get {
                return this;
            }
        }

        IControllerExpression IRouteDestinationExpression.Controller
            (Expression<Func<IServiceLocator, object>> controllerFunc)
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

        void IStaticFilesExpression.FromResource(Func<VirtualPath, IServiceLocator, IResource> locator)
        {
            var target = new RouteTarget(svc => new StaticFileController(svc, locator));
            _routes.Add(_path.Append("{path*}"), target);
        }
    }
}
