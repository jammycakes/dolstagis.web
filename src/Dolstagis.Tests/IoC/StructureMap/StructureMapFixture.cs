using Dolstagis.Tests.IoC.Common;
using Dolstagis.Web.StructureMap;

namespace Dolstagis.Tests.IoC.StructureMap
{
    public class StructureMapFixture : ContainerFixture<StructureMapContainer>
    {
        protected override StructureMapContainer CreateContainer()
        {
            return new StructureMapContainer();
        }
    }
}
