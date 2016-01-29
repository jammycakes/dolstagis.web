using System;
using System.Linq.Expressions;

namespace Dolstagis.Web.Features
{
    public interface IControllerExpression
    {
        IControllerExpression WithModelBinder(Expression<Func<IServiceProvider, IModelBinder>> modelBinderFunc);
    }
}