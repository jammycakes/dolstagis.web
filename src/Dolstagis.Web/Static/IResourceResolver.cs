namespace Dolstagis.Web.Static
{
    public interface IResourceResolver
    {
        IResource GetResource(VirtualPath path);
    }
}