using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
