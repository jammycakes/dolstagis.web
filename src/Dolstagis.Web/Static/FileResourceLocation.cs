using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class FileResourceLocation : ResourceMapping
    {
        public string FileLocation { get; private set; }

        /// <summary>
        ///  Creates a new <see cref="FileResourceLocation"/> serving up files from a
        ///  virtual path within the application root directory.
        /// </summary>
        /// <param name="root">
        ///  The root URL for the files. This must be app-relative.
        /// </param>
        /// <param name="application"></param>

        public FileResourceLocation(string type, VirtualPath root, IApplicationContext application)
            : base(type, root)
        {
            var parts = new string[] { application.PhysicalPath }.Concat(root.Parts).ToArray();
            FileLocation = Path.Combine(parts);
        }

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

        public FileResourceLocation(string type, VirtualPath root, string physicalFileLocation)
            : base(type, root)
        {
            if (!Path.IsPathRooted(physicalFileLocation)) {
                throw new ArgumentException("Physical file mapping must be an absolute path.");
            }
            FileLocation = physicalFileLocation;
        }

        /// <summary>
        ///  Creates a new <see cref="FileResourceLocation"/> serving up files from a
        ///  virtual path within the application root directory, but at a different URL.
        /// </summary>
        /// <param name="root">
        ///  The root URL for the files. This must be app-relative.
        /// </param>
        /// <param name="virtualFileLocation">
        ///  The mapping in the application where the files are located. This may be either
        ///  app-relative or request-relative; in the latter case, it will be taken as being
        ///  relative to the root.
        /// </param>
        /// <param name="application"></param>

        public FileResourceLocation(string type, VirtualPath root, VirtualPath virtualFileLocation, IApplicationContext application)
            : base(type, root)
        {
            switch (virtualFileLocation.Type) {
                case VirtualPathType.Absolute:
                    throw new ArgumentException("The file mapping must be within the application.", "fileLocation");
                case VirtualPathType.RequestRelative:
                    virtualFileLocation = root.Append(virtualFileLocation);
                    break;
            }

            var parts = new string[] { application.PhysicalPath }.Concat(virtualFileLocation.Parts).ToArray();
            FileLocation = Path.Combine(parts);
        }

        protected override IResource CreateResource(VirtualPath path)
        {
            var parts = new string[] { FileLocation }.Concat(path.Parts).ToArray();
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
