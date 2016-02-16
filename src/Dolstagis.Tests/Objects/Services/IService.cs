using System;

namespace Dolstagis.Tests.Objects.Services
{
    public interface IService : IDisposable
    {
        int DisposeCount { get; }

        IDependency Dependency { get; }

        string Name { get; }
    }
}
