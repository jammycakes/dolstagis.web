using System;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.IoC
{
    public static class IoCExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }
    }
}
