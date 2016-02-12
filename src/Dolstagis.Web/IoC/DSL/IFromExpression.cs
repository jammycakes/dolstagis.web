using System;

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
