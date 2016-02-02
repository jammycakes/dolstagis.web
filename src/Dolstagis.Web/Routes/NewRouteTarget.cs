using System;
using System.IO;
using System.Linq.Expressions;
using Dolstagis.Web.Features;

namespace Dolstagis.Web.Routes
{
    public class NewRouteTarget : IRouteTarget, IRouteExpression,
        IRouteDestinationExpression, IControllerExpression, IStaticFilesExpression
    {
        public Expression<Func<IServiceProvider, object>> GetController { get; private set; }

        public Expression<Func<IServiceProvider, IModelBinder>> GetModelBinder { get; private set; }

        public Type ControllerType { get; private set; }

        IRouteDestinationExpression IRouteExpression.To
        {
            get { return this; }
        }

        IStaticFilesExpression IRouteDestinationExpression.StaticFiles
        {
            get { return this; }
        }

        IControllerExpression IRouteDestinationExpression.Controller(Expression<Func<IServiceProvider, object>> controllerFunc)
        {
            GetController = controllerFunc;
            return this;
        }

        IControllerExpression IControllerExpression.WithModelBinder(Expression<Func<IServiceProvider, IModelBinder>> modelBinderFunc)
        {
            GetModelBinder = modelBinderFunc;
            return this;
        }


        void IStaticFilesExpression.FromStream(Func<VirtualPath, IServiceProvider, Stream> streamLocator)
        {
            throw new NotImplementedException();
        }
    }
}
