namespace Dolstagis.Tests.Objects.Services
{
    public class Service : IService
    {
        public Service(IDependency dependency)
        {
            Dependency = dependency;
            DisposeCount = 0;
        }

        public IDependency Dependency { get; private set; }

        public int DisposeCount { get; private set; }

        public string Name { get; set; }

        public void Dispose()
        {
            DisposeCount++;
        }
    }
}
