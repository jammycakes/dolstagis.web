using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.Razor
{
    public class RazorView : IView
    {
        private RazorViewEngine _engine;
        private VirtualPath _pathToView;
        private IResource _resource;
        private ResourceResolver _resolver;

        public RazorView(RazorViewEngine razorViewEngine, VirtualPath pathToView, IResource resource, ResourceResolver resolver)
        {
            this._engine = razorViewEngine;
            this._pathToView = pathToView;
            this._resource = resource;
            this._resolver = resolver;
        }

        public async Task Render(Stream stream, ViewResult data)
        {
            await Task.Yield();
        }
    }
}
