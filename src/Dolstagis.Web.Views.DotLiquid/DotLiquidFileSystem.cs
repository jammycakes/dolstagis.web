using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;
using DotLiquid;
using DotLiquid.FileSystems;

namespace Dolstagis.Web.Views.DotLiquid
{
    public class DotLiquidFileSystem : IFileSystem
    {
        private ResourceResolver _resolver;

        public DotLiquidFileSystem(ResourceResolver resolver)
        {
            _resolver = resolver;
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            var resource = _resolver.GetResource(templateName);
            using (var stream = resource.Open())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
