using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace Dolstagis.Web.StructureMap
{
    public class StructureMapContainer : IIoCContainer<IContainer>
    {
        private IContainer _container;

        public StructureMapContainer()
        {
            this._container = new Container();
        }

        public StructureMapContainer(IContainer container)
        {
            this._container = container;
        }

        public IContainer Container
        {
            get {
                return _container;
            }
        }

        public virtual IIoCContainer GetChildContainer()
        {
            return new StructureMapDomainContainer(_container.CreateChildContainer());
        }

        public object GetService(Type serviceType)
        {
            if (typeof(IEnumerable).IsAssignableFrom(serviceType)) {
                return _container.GetAllInstances(serviceType);
            }
            else {
                return _container.GetInstance(serviceType);
            }
        }

        private class StructureMapDomainContainer : StructureMapContainer
        {
            public StructureMapDomainContainer(IContainer container)
                : base(container)
            { }

            public override IIoCContainer GetChildContainer()
            {
                return new StructureMapContainer(_container.GetNestedContainer());
            }
        }
    }
}
