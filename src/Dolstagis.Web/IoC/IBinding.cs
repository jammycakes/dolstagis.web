using System;

namespace Dolstagis.Web.IoC
{
    public interface IBinding
    {
        Type SourceType { get; }

        Type TargetType { get; }

        object Target { get; }

        Func<IIoCContainer, object> TargetFunc { get; }

        bool Transient { get; }

        bool Multiple { get; }
    }
}
