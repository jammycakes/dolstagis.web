using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;
using System.Web.Razor.Generator;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorViewEngine : ViewEngineBase
    {
        private readonly string _extension;
        private readonly RazorCodeLanguage _language;
        private readonly RazorEngineHost _host;

        public RazorViewEngine(string extension, RazorCodeLanguage language)
        {
            _extension = extension;
            _language = language;
            _host = new RazorEngineHost(language)
            {
                DefaultBaseClass = typeof(TemplateBase).FullName,
                DefaultNamespace = "Dolstagis.Web.Views.Razor.Compiled",
                GeneratedClassContext = new GeneratedClassContext(
                    executeMethodName: "Execute",
                    writeMethodName: "Write",
                    writeLiteralMethodName: "WriteLiteral",
                    writeToMethodName: "WriteTo",
                    writeLiteralToMethodName: "WriteLiteralTo",
                    templateTypeName: null,
                    defineSectionMethodName: "DefineSection"
                )
            };
            _host.NamespaceImports.Add("System");
            _host.NamespaceImports.Add("System.Collections.Generic");
            _host.NamespaceImports.Add("System.Linq");
            _host.NamespaceImports.Add("System.Text");
            _host.NamespaceImports.Add("System.Threading.Tasks");
        }

        public override IEnumerable<string> Extensions
        {
            get { return new[] { _extension }; }
        }

        protected override IView CreateView(VirtualPath pathToView, IResourceResolver resolver)
        {
            var resource = resolver.GetResource(pathToView);
            if (resource == null || !resource.Exists) return null;

            return new RazorView(this, pathToView, resource, resolver);
        }
    }
}
