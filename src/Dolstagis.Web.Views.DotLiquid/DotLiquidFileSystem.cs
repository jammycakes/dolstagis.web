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
        public static string Guid { get; private set; }

        static DotLiquidFileSystem()
        {
            Guid = System.Guid.NewGuid().ToString();
        }

        public string ReadTemplateFile(Context context, string templateName)
        {
            var templatePath = (string)context[templateName];

            var resolver = context.Registers[Guid] as ResourceResolver;

            var resource = resolver.GetResource("~/" + templatePath + ".liquid");
            using (var stream = resource.Open())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
