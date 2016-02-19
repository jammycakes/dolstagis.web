using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    public class ViewResourceResolver : IResourceResolver
    {
        private ViewRegistry _registry;
        private IServiceLocator _serviceLocator;
        private IResource _initialResource;

        public ViewResourceResolver(ViewRegistry registry, IServiceLocator locator, IResource initialResource)
        {
            _registry = registry;
            _serviceLocator = locator;
            _initialResource = initialResource;
        }

        public IResource GetResource(VirtualPath path)
        {
            if (_initialResource != null) {
                var result = _initialResource;
                _initialResource = null;
                return result;
            }

            var info = _registry.GetViewInfo(path);
            return info.Location(info.RelativePath, _serviceLocator);
        }
    }
}
