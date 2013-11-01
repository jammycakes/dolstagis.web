using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public static class ApplicationContextExtensions
    {
        public static string MapPath(this IApplicationContext context, VirtualPath vPath)
        {
            switch (vPath.Type)
            {
                case VirtualPathType.RequestRelative:
                    throw new ArgumentOutOfRangeException("vPath", "Path must be absolute or app-relative.");
                case VirtualPathType.Absolute:
                    vPath = context.VirtualPath.GetAppRelativePath(vPath, true);
                    if (vPath == null) {
                        throw new ArgumentOutOfRangeException("vPath", "Path is not within the application.");
                    }
                    break;
            }

            var parts = new string[] { context.PhysicalPath }.Concat(vPath.Parts).ToArray();
            return Path.Combine(parts);
        }
    }
}
