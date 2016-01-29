using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Features
{
    public interface IRouteDestinationExpression
    {
        IHandlerExpression Handler(Expression<Func<IServiceProvider, object>> handlerFunc);

        IStaticFilesExpression StaticFiles { get; }
    }
}