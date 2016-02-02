using System;

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
    }

    public interface IIoCContainer<TImpl> : IIoCContainer
    {
        TImpl Container { get; }
    }
}
