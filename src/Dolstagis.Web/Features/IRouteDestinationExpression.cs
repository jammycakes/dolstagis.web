using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Features
{
    public interface IRouteDestinationExpression
    {
        IControllerExpression Controller(Expression<Func<IServiceProvider, object>> controllerFunc);

        IStaticFilesExpression StaticFiles { get; }
    }
}