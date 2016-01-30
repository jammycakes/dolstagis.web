using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Features.Impl
{
    public class ContainerConfiguration : IContainerExpression
    {
        public IContainerBuilder Builder { get; private set; }

        public IContainerIsExpression<TContainer> Is<TContainer>()
            where TContainer : class, IIoCContainer, new()
        {
            var cb = new ContainerBuilder<TContainer>();
            Builder = cb;
            return cb;
        }
    }
}
