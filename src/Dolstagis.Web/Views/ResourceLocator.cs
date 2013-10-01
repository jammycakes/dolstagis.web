using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Routing;

namespace Dolstagis.Web.Views
{
    public class ResourceLocator
    {
        public IDictionary<string, IList<IResourceLocation>> _locations
            = new Dictionary<string, IList<IResourceLocation>>(StringComparer.OrdinalIgnoreCase);

        public ResourceLocator()
        {
        }

        /// <summary>
        ///  Adds a directory or file relative to the application root directory.
        /// </summary>
        /// <param name="baseUrl">
        ///  The base URl of the resource or resource directory, relative to the application.
        /// </param>

        public void Add(string baseUrl)
        {
            Add(baseUrl, baseUrl);
        }

        /// <summary>
        ///  Adds a directory or file with a specified base URL and physical path.
        /// </summary>
        /// <param name="baseUrl">
        ///  The base URL of the resource or resource directory, relative to the application.
        /// </param>
        /// <param name="physicalPath">
        ///  The physical path to the resource.
        /// </param>

        public void Add(string baseUrl, string physicalPath)
        {
            Add(baseUrl, new FilespaceResourceLocation(baseUrl.NormaliseUrlPath()));
        }

        /// <summary>
        ///  Adds a directory or file with a specified base URL and location.
        /// </summary>
        /// <param name="baseUrl">
        ///  The base URL of the resource or resource directory, relative to the application.
        /// </param>
        /// <param name="location">
        ///  An <see cref="IResourceLocation"/> instance which will be used to find the resource.
        /// </param>

        public void Add(string baseUrl, IResourceLocation location)
        {
            baseUrl = baseUrl.NormaliseUrlPath();
            IList<IResourceLocation> locationList;
            if (!_locations.TryGetValue(baseUrl, out locationList)) {
                locationList = new List<IResourceLocation>();
                _locations[baseUrl] = locationList;
            }
            locationList.Add(location);
        }

        /// <summary>
        ///  Opens a stream to read the resource.
        /// </summary>
        /// <param name="path">
        ///  The virtual path to the resource.
        /// </param>
        /// <param name="appRoot">
        ///  The application's root directory.
        /// </param>
        /// <returns>
        ///  A <see cref="Stream"/> object, or null if the resource was not found.
        /// </returns>

        public IResource Get(string path, string appRoot)
        {
            var parts = path.SplitUrlPath();
            string locatorPath, resourcePath;
            for (var i = parts.Length - 1; i >= 0; i--) {
                locatorPath = String.Join("/", parts.Take(i).ToArray());
                resourcePath = String.Join("/", parts.Skip(i).ToArray());
                IList<IResourceLocation> locationList;
                if (_locations.TryGetValue(locatorPath, out locationList)) {
                    foreach (var location in locationList) {
                        var resource = location.Get(resourcePath, appRoot);
                        if (resource != null) return resource;
                    }
                }
            }
            return null;
        }
    }
}
