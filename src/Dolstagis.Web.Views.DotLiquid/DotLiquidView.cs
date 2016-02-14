using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dolstagis.Web.Static;
using DotLiquid;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidView : IView
    {
        static DotLiquidView()
        {
            Template.RegisterSafeType(typeof(ViewData), new string[] { "Data", "Model", "Status" });
            Template.RegisterSafeType(typeof(Status), new string[] { "Code", "Description", "Message" });
        }

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

        public Task Render(Stream stream, ViewData data)
        {
            var register = new Hash();
            register.Add(DotLiquidFileSystem.Guid, _resolver);

            var hash = new Hash();
            hash.Add("Data", data.Data);
            hash.Add("Model", data.Model);
            hash.Add("Status", data.Status);
            var ctx = new Context(new List<Hash>(), hash, register, true);

            using (var writer = new StreamWriter(stream, data.Encoding)) {
                _template.Render(writer, new RenderParameters {
                    Context = ctx
                });
            }
            return Task.FromResult(0);
        }
    }
}
