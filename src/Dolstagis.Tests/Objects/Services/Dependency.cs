namespace Dolstagis.Tests.Objects.Services
{
    public class Dependency : IDependency
    {
        public static int ConstructorCallCount { get; set; }

        public Dependency()
        {
            ConstructorCallCount++;
        }

        public int DisposeCount { get; private set; } = 0;

        public string Name { get; set; }

        public void Dispose()
        {
            this.DisposeCount += 1;
        }
    }
}
