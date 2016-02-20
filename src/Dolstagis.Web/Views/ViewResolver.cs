using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.IoC;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views
{
    /// <summary>
    ///  The ViewResolver is the class that co-ordinates the resolution and
    ///  construction of views for the request.
    /// </summary>
    /// <remarks>
    ///  One ViewResolver is created for each request. It depends on the view
    ///  registry (which has singleton scope) and the IOC container (which has
    ///  request scope).
    /// </remarks>

    public class ViewResolver : IViewResolver
    {
        private ViewRegistry _registry;
        private IServiceLocator _serviceLocator;

        public ViewResolver(ViewRegistry registry, IServiceLocator serviceLocator)
        {
            _registry = registry;
            _serviceLocator = serviceLocator;
        }


        public IView GetView(VirtualPath path)
        {
            if (path.Parts.Count == 0) return null;
            var filename = "../" + path.Parts.Last();

            var candidatePaths = new VirtualPath[] { path }
                .Concat(
                    from ext in _registry.GetRegisteredExtensions()
                    select path.Append(filename + "." + ext)
                );

            var infos =
                from c in candidatePaths
                let info = _registry.GetViewInfo(path)
                let resource = info.Location(info.RelativePath, _serviceLocator)
                where resource != null && resource.IsFile
                select new {
                    Path = c,
                    Resource = resource
                };

            var viewInfo = infos.FirstOrDefault();
            if (viewInfo == null) return null;

            var resourceResolver = new ViewResourceResolver(_registry, _serviceLocator, viewInfo.Resource);
            var extension = viewInfo.Path.Parts.Last().Split('.').Last();
            var viewEngine = _registry.GetViewEngine(extension);
            return viewEngine.GetView(viewInfo.Path, resourceResolver);
        }
    }
}
