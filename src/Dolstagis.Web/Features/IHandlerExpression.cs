using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Features
{
    public interface IHandlerExpression
    {
        IHandlerExpression WithModelBinder(Expression<Func<IServiceProvider, IModelBinder>> modelBinderFunc);
    }
}