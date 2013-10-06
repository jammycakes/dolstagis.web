using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Views
{
    public class ViewResultProcessor : ResultProcessor<ViewResult>
    {
        private ViewEngineRegistry _registry;

        public ViewResultProcessor(ViewEngineRegistry registry)
        {
            _registry = registry;
        }

        public override Task Process(ViewResult data, IRequestContext context)
        {
            throw new NotImplementedException();
        }
    }
}
