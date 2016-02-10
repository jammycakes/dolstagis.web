using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.IoC
{
    public static class IoCExtensions
    {
        public static TService Get<TService>(this IServiceLocator serviceLocator)
        {
            return (TService)serviceLocator.Get(typeof(TService));
        }

        public static IEnumerable<TService> GetAll<TService>(this IServiceLocator serviceLocator)
        {
            return serviceLocator.GetAll(typeof(TService)).Cast<TService>();
        }
    }
}
