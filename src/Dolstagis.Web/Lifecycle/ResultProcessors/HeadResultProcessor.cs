using System.Threading.Tasks;

namespace Dolstagis.Web.Lifecycle.ResultProcessors
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
