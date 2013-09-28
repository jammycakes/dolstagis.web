using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Http;
using StructureMap;

namespace Dolstagis.Web
{
    public class Application : IDisposable
    {
        public void Init()
        {
        }

        public void AddModule<T>(T module) where T: Module, new()
        {
        }

        public void ProcessRequest(IHttpContext context)
        {
            ProcessRequestAsync(context).Wait();
        }

        public async Task ProcessRequestAsync(IHttpContext context)
        {
        }

        public void Dispose()
        {
        }
    }
}
