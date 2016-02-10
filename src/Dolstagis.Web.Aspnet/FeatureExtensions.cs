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


        /// <summary>
        ///  Registers a directory of views, or a single view file,
        ///  within the filespace of the ASP.NET hosting environment.
        /// </summary>
        /// <param name="feature">
        ///  The feature in which the files are to be registered.
        /// </param>
        /// <param name="path">
        ///  The virtual path to the view(s) to be registered.
        /// </param>

        public static void AddViews(this Feature feature, VirtualPath path)
        {
            feature.AddViews(path, HostingEnvironment.MapPath(path.ToString()));
        }
    }
}