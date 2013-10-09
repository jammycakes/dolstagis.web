using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;
using global::Nustache.Core;

namespace Dolstagis.Web.Views.Nustache
{
    public class NustacheView : IView
    {
        private NustacheViewEngine _engine;
        private VirtualPath _path;
        public Template Template { get; private set; }


        public NustacheView(NustacheViewEngine engine, VirtualPath path, IResource resource)
        {
            _engine = engine;
            _path = path;
            using (var stream = resource.Open())
            using (var reader = new StreamReader(stream)) {
                Template = new Template();
                Template.Load(reader);
            }
        }

        public async Task Render(IRequestContext context, object data)
        {
            using (var writer = new StreamWriter(context.Response.ResponseStream)) {
                Template.Render(data, writer, null);
            }
        }
    }
}
