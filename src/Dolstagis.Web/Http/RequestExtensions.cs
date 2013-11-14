using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Http
{
    public static class RequestExtensions
    {
        public static Uri GetAbsoluteUrl(this IRequest request, VirtualPath path)
        {
            switch (path.Type)
            {
                case VirtualPathType.Absolute:
                    return new Uri(request.Url, "/" + path.Path);
                case VirtualPathType.AppRelative:
                    return new Uri(request.Url, "/" + request.AppRelativePath.Append(path).Path);
                case VirtualPathType.RequestRelative:
                    return new Uri(request.Url, path.Path);
                default:
                    throw new ArgumentException("The path has an invalid type.", "path");
            }
        }
    }
}
