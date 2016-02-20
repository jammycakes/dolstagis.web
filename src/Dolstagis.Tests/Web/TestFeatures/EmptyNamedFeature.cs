using System;
using Dolstagis.Web;
using Dolstagis.Web.Http;

namespace Dolstagis.Tests.Web.TestFeatures
{
    public class EmptyNamedFeature : Feature
    {
        public string Name { get; private set; }

        public EmptyNamedFeature(string name)
        {
            this.Name = name;
        }

        public EmptyNamedFeature(string name, Predicate<IRequest> condition)
        {
            Name = name;
            Activate.When(condition);
        }
    }
}
