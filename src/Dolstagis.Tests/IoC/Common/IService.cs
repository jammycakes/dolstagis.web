using System;

namespace Dolstagis.Tests.IoC.Common
{
    public interface IService : IDisposable
    {
        int DisposeCount { get; }

        IDependency Dependency { get; }
    }
}
