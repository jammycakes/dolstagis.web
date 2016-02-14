using System;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using Dolstagis.Web.Static;
using global::Nustache.Core;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheView : IView
    {
        private NustacheViewEngine _engine;
        private VirtualPath _path;
        private IResourceResolver _resolver;

        public Template Template { get; private set; }


        public NustacheView(NustacheViewEngine engine, VirtualPath path, IResource resource, IResourceResolver resolver)
        {
            _engine = engine;
            _path = path;
            _resolver = resolver;
            using (var stream = resource.Open())
            using (var reader = new StreamReader(stream)) {
                Template = new Template();
                Template.Load(reader);
            }
        }

        private Template GetChildTemplate(string path)
        {
            var newPath = new VirtualPath(path);
            if (newPath.Type != VirtualPathType.AppRelative) {
                throw new NotSupportedException("The Nustache view engine only supports app-relative paths at present.");
            }
            var result = _engine.GetView(newPath, _resolver) as NustacheView;
            if (result == null) return null;
            return result.Template;
        }

        public Task Render(IResponse response, ViewData data)
        {
            using (var writer = response.GetStreamWriter()) {
                Template.Render(data, writer, GetChildTemplate);
            }
            return Task.FromResult(0);
        }
    }
}
