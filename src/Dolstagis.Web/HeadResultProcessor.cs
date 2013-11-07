using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web
{
    public class HeadResultProcessor : ResultProcessor<HeadResult>
    {
        public override async Task Process(HeadResult data, IHttpContext context)
        {
            await Task.Yield();
        }
    }
}
