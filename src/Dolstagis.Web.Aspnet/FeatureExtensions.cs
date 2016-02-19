using System.Web.Hosting;
using Dolstagis.Web.Features;

namespace Dolstagis.Web.Aspnet
{
    public static class FeatureExtensions
    {
        public static void FromWebApplication(this IStaticFilesExpression expr, VirtualPath path)
        {
            expr.FromDirectory(HostingEnvironment.MapPath(path.ToString()));
        }
    }
}