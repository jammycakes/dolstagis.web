namespace Dolstagis.Web.Static
{
    public interface IResourceLocation
    {
        IResource GetResource(VirtualPath path);
    }
}
