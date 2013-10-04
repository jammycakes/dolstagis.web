using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;

namespace Dolstagis.Web.Views.Static
{
    public class StaticHandler : Handler
    {
        private IRequest _request;

        public StaticHandler(IRequest request)
        {
            _request = request;
        }

        public object Get(string path = "")
        {
            return new StaticResult(_request.AppRelativePath.Path);
        }
    }
}
