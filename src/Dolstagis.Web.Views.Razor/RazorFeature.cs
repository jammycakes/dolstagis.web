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
        public RazorFeature()
        {
            this.Services.For<IViewEngine>().Singleton().Add<RazorViewEngine>()
                .Ctor<string>("extension").Is("cshtml")
                .Ctor<RazorCodeLanguage>("language").Is(new CSharpRazorCodeLanguage());
            this.Services.For<IViewEngine>().Singleton().Add<RazorViewEngine>()
                .Ctor<string>("extension").Is("vbhtml")
                .Ctor<RazorCodeLanguage>("language").Is(new VBRazorCodeLanguage());

        }
    }
}
