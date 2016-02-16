using System;

namespace Dolstagis.Web.IoC
{
    public interface IContainerBuilder
    {
        bool HasInstance { get; }
        IIoCContainer Instance { get; }
        Type ContainerType { get; }
        IIoCContainer CreateContainer();
        void SetupApplication(IIoCContainer container);
        void SetupRequest(IIoCContainer container);

        event EventHandler SettingContainer;
    }
}