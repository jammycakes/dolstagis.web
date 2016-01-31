using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Features.Impl
{
    public class ContainerConfiguration : IContainerExpression
    {
        private IContainerBuilder _builder;
        private IContainerSetupExpression<IIoCContainer> _builderExpression;

        public ContainerConfiguration()
        {
            var cb = new ContainerBuilder<IIoCContainer>();
            _builder = cb;
            _builderExpression = cb;
        }


        public IContainerBuilder Builder { get { return _builder; } }

        IContainerSetupExpression<IIoCContainer> IContainerUsingExpression<IIoCContainer>.Setup
        {
            get {
                return _builderExpression;
            }
        }

        IContainerIsExpression<TContainer> IContainerExpression.Is<TContainer>()
        {
            var cb = new ContainerBuilder<TContainer>();
            _builder = cb;
            _builderExpression = cb;
            return cb;
        }
    }
}
