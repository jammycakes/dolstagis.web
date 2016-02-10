using System;

namespace Dolstagis.Web.IoC
{
    public interface IIoCContainer : IServiceProvider, IDisposable
    {
        IIoCContainer GetChildContainer();

        void Add(IBinding binding);

        void Validate();
    }

    public interface IIoCContainer<TImpl> : IIoCContainer
    {
        TImpl Container { get; }
    }
}
