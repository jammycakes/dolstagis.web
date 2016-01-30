namespace Dolstagis.Tests.IoC.Common
{
    public class Dependency : IDependency
    {
        public static int ConstructorCallCount { get; set; }

        public Dependency()
        {
            ConstructorCallCount++;
        }

        public int DisposeCount { get; private set; } = 0;

        public void Dispose()
        {
            this.DisposeCount += 1;
        }
    }
}
