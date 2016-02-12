namespace Dolstagis.Web.Static
{
    public class ResourceResult : ResultBase
    {
        public IResource Resource { get; private set; }

        public ResourceResult(IResource resource)
        {
            Resource = resource;
        }
    }
}
