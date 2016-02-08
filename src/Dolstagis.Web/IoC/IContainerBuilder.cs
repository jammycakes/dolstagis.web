using System;
using Dolstagis.Web.IoC.DSL;

namespace Dolstagis.Web.IoC
{
    public interface IContainerBuilder
    {
        bool HasInstance { get; }
        IIoCContainer Instance { get; }
        Type ContainerType { get; }
        bool ApplicationLevel { get; }
        IIoCContainer CreateContainer();
        void SetupApplication(IIoCContainer container);
        void SetupFeature(IIoCContainer container);
        void SetupRequest(IIoCContainer container);

        event EventHandler ConfiguringApplication;
        event EventHandler SettingContainer;
    }
}