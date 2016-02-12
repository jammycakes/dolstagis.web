using System.Threading.Tasks;
using Dolstagis.Web.Lifecycle;

namespace Dolstagis.Web.Views
{
    public class ViewResultProcessor : ResultProcessor<ViewResult>
    {
        private ViewRegistry _registry;

        public ViewResultProcessor(ViewRegistry registry)
        {
            _registry = registry;
        }

        protected override async Task ProcessTypedBodyAsync(ViewResult data, IRequestContext context)
        {
            var view = _registry.GetView(data.Path);
            await view.Render(context.Response.Body, data);
        }
    }
}
