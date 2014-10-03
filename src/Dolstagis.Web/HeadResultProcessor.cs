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
        public static readonly HeadResultProcessor Instance = new HeadResultProcessor();

        private HeadResultProcessor()
        { }

        public override async Task ProcessBody(HeadResult data, IRequestContext context)
        {
            await Task.Yield();
        }
    }
}
