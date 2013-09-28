using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dolstagis.Web.Aspnet
{
    public class HttpRequestHandler : HttpTaskAsyncHandler
    {
        private static Application _application;

        public override bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            EnsureInit();
            var requestContext = new HttpRequestContext(new HttpContextWrapper(context));
            await _application.ProcessRequestAsync(requestContext);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void EnsureInit()
        {
            if (_application != null) return;
            _application = new Application();
            _application.Init();
        }
    }
}
