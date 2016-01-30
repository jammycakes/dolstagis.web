using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IIoCContainer : IServiceProvider
    {
        IIoCContainer GetChildContainer();
    }

    public interface IIoCContainer<TImpl> : IIoCContainer
    {
        TImpl Container { get; }
    }
}
