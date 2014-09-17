using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Dolstagis.Web.Aspnet
{
    public static class FeatureExtensions
    {
        /// <summary>
        ///  Registers a directory of static files, or a single static file,
        ///  within the filespace of the ASP.NET hosting environment.
        /// </summary>
        /// <param name="feature">
        ///  The feature in which the files are to be registered.
        /// </param>
        /// <param name="path">
        ///  The virtual path to the file or directory to be registered.
        /// </param>

        public static void AddStaticFiles(this Feature feature, VirtualPath path)
        {
            feature.AddStaticFiles(path, HostingEnvironment.MapPath(path.ToString()));
        }


        /// <summary>
        ///  Registers a directory of static files, or a single static file,
        ///  within the filespace of the ASP.NET hosting environment.
        /// </summary>
        /// <param name="feature">
        ///  The feature in which the files are to be registered.
        /// </param>
        /// <param name="path">
        ///  The virtual path to the file or directory to be registered.
        /// </param>

        public static void AddStaticFiles(this Feature feature, string path)
        {
            feature.AddStaticFiles(path, HostingEnvironment.MapPath(path));
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

        public static void AddViews(this Feature feature, string path)
        {
            feature.AddViews(path, HostingEnvironment.MapPath(path));
        }
    }
}