using System;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Static
{
    public class StaticFileController
    {
        private IServiceLocator _serviceProvider;
        private Func<VirtualPath, IServiceLocator, IResource> _resourceFunc;

        public StaticFileController(IServiceLocator provider,
            Func<VirtualPath, IServiceLocator, IResource> resourceFunc)
        {
            _serviceProvider = provider;
            _resourceFunc = resourceFunc;
        }

        public object Get(string path = "")
        {
            return new ResourceResult(_resourceFunc(path, _serviceProvider));
        }
    }
}
