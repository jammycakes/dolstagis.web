using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.IoC.DSL
{
    public interface IFromExpression<TSource>
    {
        IFromExpression<TSource> Only();

        IToExpression To<TTarget>() where TTarget : class, TSource;

        IToExpression To<TTarget>(Func<IIoCContainer, TTarget> targetFunc)
            where TTarget : TSource;

        IToExpression To(TSource target);
    }
}
