using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorFeature : Feature
    {
        private IViewEngine CreateViewEngine<TLanguage>(IIoCContainer container, string extension)
            where TLanguage: RazorCodeLanguage
        {
            return new RazorViewEngine(
                container.GetService<ISettings>(),
                extension,
                container.GetService<TLanguage>()
            );
        }

        public RazorFeature()
        {
            Container.Setup.Feature(c => {
                c.Add<IViewEngine>(ctr => CreateViewEngine<CSharpRazorCodeLanguage>(ctr, "cshtml"), Scope.Application);
                c.Add<IViewEngine>(ctr => CreateViewEngine<VBRazorCodeLanguage>(ctr, "vbhtml"), Scope.Application);
            });
        }
    }
}
