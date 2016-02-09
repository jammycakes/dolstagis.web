using System;

namespace Dolstagis.Web.IoC
{
    public interface IIoCContainer : IServiceProvider, IDisposable
    {
        IIoCContainer GetChildContainer();

        void Add(IBinding binding);

        // TODO: all other Add/Use methods need to be deprecated.

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
