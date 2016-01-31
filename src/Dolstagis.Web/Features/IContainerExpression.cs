using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Features
{
    public interface IContainerExpression : IContainerUsingExpression<IIoCContainer>
    {
        IContainerIsExpression<TContainer> Is<TContainer>()
            where TContainer : class, IIoCContainer, new();
    }
}
