using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routes;

namespace Dolstagis.Web.Features.Impl
{
    public class RouteExpression : IRouteExpression, IRouteDestinationExpression
    {
        private RouteTable _routes;
        private VirtualPath _path;

        public RouteExpression(RouteTable routes, VirtualPath path)
        {
            _routes = routes;
            _path = path;
        }

        public IRouteDestinationExpression To
        {
            get {
                return this;
            }
        }

        public IControllerExpression Controller(Expression<Func<IServiceProvider, object>> controllerFunc)
        {
            var target = new RouteTarget(controllerFunc);
            _routes.Add(_path, target);
            return target;
        }

        public IStaticFilesExpression StaticFiles
        {
            get {
                throw new NotImplementedException();
            }
        }
    }
}
