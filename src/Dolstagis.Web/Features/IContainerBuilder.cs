using System;

namespace Dolstagis.Web.Features
{
    public interface IContainerBuilder
    {
        bool HasInstance { get; }
        IIoCContainer Instance { get; }
        Type ContainerType { get; }
        IIoCContainer CreateContainer();
        void SetupApplication(IIoCContainer container);
        void SetupDomain(IIoCContainer container);
        void SetupRequest(IIoCContainer container);
    }
}