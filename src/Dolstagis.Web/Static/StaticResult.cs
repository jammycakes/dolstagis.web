namespace Dolstagis.Web.Static
{
    public class StaticResult : ResultBase
    {
        public VirtualPath Path { get; private set; }

        public StaticResult(VirtualPath path)
        {
            this.Path = path;
        }
    }
}
