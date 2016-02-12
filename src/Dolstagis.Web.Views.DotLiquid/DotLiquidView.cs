using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Static;
using DotLiquid;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidView : IView
    {
        private VirtualPath _path;
        private IResourceResolver _resolver;
        private Template _template;

        public DotLiquidView(VirtualPath path, IResource resource, IResourceResolver resolver)
        {
            _path = path;
            _resolver = resolver;
            using (var stream = resource.Open())
            using (var reader = new StreamReader(stream)) {
                string tpl = reader.ReadToEnd();
                _template = Template.Parse(tpl);
            }
        }

        public async Task Render(Stream stream, ViewResult data)
        {
            var register = new Hash();
            register.Add(DotLiquidFileSystem.Guid, _resolver);

            var hash = new Hash();
            hash.Add("Data", data.Data);
            hash.Add("Model", data.Model);
            var ctx = new Context(new List<Hash>(), hash, register, true);

            using (var writer = new StreamWriter(stream, data.Encoding)) {
                _template.Render(writer, new RenderParameters {
                    Context = ctx
                });
            }
            await Task.Yield();
        }
    }
}
