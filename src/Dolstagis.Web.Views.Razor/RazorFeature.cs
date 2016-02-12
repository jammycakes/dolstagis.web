using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;
using Dolstagis.Web.IoC;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorFeature : Feature
    {
        private IViewEngine CreateViewEngine<TLanguage>(IIoCContainer container, string extension)
            where TLanguage: RazorCodeLanguage
        {
            return new RazorViewEngine(
                container.Get<ISettings>(),
                extension,
                container.Get<TLanguage>()
            );
        }

        public RazorFeature()
        {
            Container.Setup.Application.Bindings(c => {
                c.From<IViewEngine>()
                    .To(ctr => CreateViewEngine<CSharpRazorCodeLanguage>(ctr, "cshtml"))
                    .Managed();
                c.From<IViewEngine>()
                    .To(ctr => CreateViewEngine<VBRazorCodeLanguage>(ctr, "vbhtml"))
                    .Managed();
            });
        }
    }
}
