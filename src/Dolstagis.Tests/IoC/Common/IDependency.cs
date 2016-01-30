using System;

namespace Dolstagis.Tests.IoC.Common
{
    public interface IDependency : IDisposable
    {
        int DisposeCount { get; }
    }
}
