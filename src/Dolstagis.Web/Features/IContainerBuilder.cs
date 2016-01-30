namespace Dolstagis.Web.Features
{
    public interface IContainerBuilder
    {
        IIoCContainer GetContainer(IIoCContainer existing);
        void SetupApplication(IIoCContainer container);
        void SetupDomain(IIoCContainer container);
        void SetupRequest(IIoCContainer container);
    }
}