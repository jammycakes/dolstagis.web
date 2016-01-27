namespace Dolstagis.Web.Static
{
    public class StaticRequestHandler : Handler
    {
        public object Get(string path = "")
        {
            return new StaticResult(Context.Request.Path);
        }
    }
}
