using Dolstagis.Web;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class EmptyNamedFeature : Feature
    {
        public string Name { get; private set; }

        public EmptyNamedFeature(string name)
        {
            this.Name = name;
        }
    }
}
