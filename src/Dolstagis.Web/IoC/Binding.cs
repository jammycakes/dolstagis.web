using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Web.IoC
{
    public class Binding<TSource> :
        IBinding, IFromExpression<TSource>, IToExpression
    {
        public Binding()
        {
            Multiple = true;
            Target = null;
            TargetFunc = null;
            TargetType = (typeof(TSource).IsClass || typeof(TSource).IsValueType)
                && !typeof(TSource).IsAbstract
                ? typeof(TSource)
                : null;
            Transient = false;
        }

        public static Binding<TSource> From(Action<IFromExpression<TSource>> cfg)
        {
            var result = new Binding<TSource>();
            cfg(result);
            return result;
        }

        /* ====== Implementation of IBinding ====== */

        public bool Multiple { get; private set; }

        public Type SourceType { get { return typeof(TSource); } }

        public object Target { get; private set; }

        public Func<IIoCContainer, object> TargetFunc { get; private set; }

        public Type TargetType { get; private set; }

        public bool Transient { get; private set; }

        /* ====== DSL interface implementation ====== */

        IFromExpression<TSource> IFromExpression<TSource>.Only()
        {
            Multiple = false;
            return this;
        }

        IToExpression IFromExpression<TSource>.To(TSource target)
        {
            Target = target;
            TargetType = null;
            TargetFunc = null;
            Transient = true;
            return this;
        }

        IToExpression IFromExpression<TSource>.To<TTarget>()
        {
            Target = null;
            TargetType = typeof(TTarget);
            TargetFunc = null;
            Transient = false;
            return this;
        }

        IToExpression IFromExpression<TSource>.To<TTarget>(Func<IIoCContainer, TTarget> targetFunc)
        {
            Target = null;
            TargetType = null;
            TargetFunc = container => targetFunc(container);
            Transient = false;
            return this;
        }

        void IToExpression.Managed()
        {
            Transient = false;
        }

        void IToExpression.Transient()
        {
            Transient = true;
        }
    }
}
