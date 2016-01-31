using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public interface IIoCContainer : IServiceProvider, IDisposable
    {
        IIoCContainer GetChildContainer();

        void Add(Type source, Type target, Scope scope);

        void Use(Type source, Type target, Scope scope);

        void Add(Type source, Func<IIoCContainer, object> target, Scope scope);

        void Use(Type source, Func<IIoCContainer, object> target, Scope scope);

        void Add(Type source, object target);

        void Use(Type source, object target);

        void Validate();
    }

    public interface IIoCContainer<TImpl> : IIoCContainer
    {
        TImpl Container { get; }
    }
}
