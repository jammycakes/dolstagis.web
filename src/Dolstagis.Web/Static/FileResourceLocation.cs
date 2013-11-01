using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class FileResourceLocation : IResourceLocation
    {
        public string Root { get; private set; }


        /// <summary>
        ///  Creates a new <see cref="FileResourceLocation"/> serving up files from a
        ///  physical path at an arbitrary mapping on the filesystem.
        /// </summary>
        /// <param name="root">
        ///  The root URL for the files. This must be app-relative.
        /// </param>
        /// <param name="physicalFileLocation">
        ///  The mapping in the filespace where the files are located.
        /// </param>

        public FileResourceLocation(string physicalFileLocation)
        {
            if (!Path.IsPathRooted(physicalFileLocation)) {
                throw new ArgumentException("Physical file mapping must be an absolute path.");
            }
            Root = physicalFileLocation;
        }


        public IResource GetResource(VirtualPath path)
        {
            var parts = new string[] { Root }.Concat(path.Parts).ToArray();
            var physicalPath = Path.Combine(parts);
            if (File.Exists(physicalPath)) {
                return new FileResource(physicalPath);
            }
            else {
                return null;
            }
        }
    }
}
