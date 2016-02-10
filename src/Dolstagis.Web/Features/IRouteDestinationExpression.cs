using System;
using System.Linq.Expressions;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Features
{
    public interface IRouteDestinationExpression
    {
        IControllerExpression Controller(Expression<Func<IServiceLocator, object>> controllerFunc);

        IStaticFilesExpression StaticFiles { get; }
    }
}