using System;

namespace Dolstagis.Web
{
    public static class IoCExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }


        public static void Add<TSource, TTarget>(this IIoCContainer container, Scope scope)
        {
            container.Add(typeof(TSource), typeof(TTarget), scope);
        }

        public static void Use<TSource, TTarget>(this IIoCContainer container, Scope scope)
        {
            container.Use(typeof(TSource), typeof(TTarget), scope);
        }
        public static void Add<TSource>(this IIoCContainer container, Func<IIoCContainer, object> target, Scope scope)
        {
            container.Add(typeof(TSource), target, scope);
        }

        public static void Use<TSource>(this IIoCContainer container, Func<IIoCContainer, object> target, Scope scope)
        {
            container.Use(typeof(TSource), target, scope);
        }

        public static void Add<TSource>(this IIoCContainer container, object target)
        {
            container.Add(typeof(TSource), target);
        }

        public static void Use<TSource>(this IIoCContainer container, object target)
        {
            container.Use(typeof(TSource), target);
        }
    }
}
