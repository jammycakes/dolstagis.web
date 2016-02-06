using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class StaticFileController
    {
        private IServiceProvider _serviceProvider;
        private Func<VirtualPath, IServiceProvider, IResource> _resourceFunc;

        public StaticFileController(IServiceProvider provider,
            Func<VirtualPath, IServiceProvider, IResource> resourceFunc)
        {
            _serviceProvider = provider;
            _resourceFunc = resourceFunc;
        }

        public object Get(string path)
        {
            return new ResourceResult(_resourceFunc(path, _serviceProvider));
        }
    }
}
