using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Features;

namespace Dolstagis.Web.Routes
{
    public class NewRouteTarget : IRouteTarget, IRouteExpression,
        IRouteDestinationExpression, IHandlerExpression, IStaticFilesExpression
    {
        public Expression<Func<IServiceProvider, object>> GetHandler { get; private set; }

        public Expression<Func<IServiceProvider, IModelBinder>> GetModelBinder { get; private set; }

        public Type HandlerType { get; private set; }

        IRouteDestinationExpression IRouteExpression.To
        {
            get { return this; }
        }

        IStaticFilesExpression IRouteDestinationExpression.StaticFiles
        {
            get { return this; }
        }

        IHandlerExpression IRouteDestinationExpression.Handler(Expression<Func<IServiceProvider, object>> handlerFunc)
        {
            GetHandler = handlerFunc;
            return this;
        }

        IHandlerExpression IHandlerExpression.WithModelBinder(Expression<Func<IServiceProvider, IModelBinder>> modelBinderFunc)
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
