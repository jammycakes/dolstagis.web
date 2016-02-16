using System;

namespace Dolstagis.Tests.Objects.Services
{
    public interface IDependency : IDisposable
    {
        int DisposeCount { get; }

        string Name { get; }
    }
}
