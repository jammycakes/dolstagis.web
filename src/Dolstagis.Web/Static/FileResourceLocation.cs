using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Static
{
    public class FileResourceLocation : ResourceLocation
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

        public FileResourceLocation(VirtualPath root, IApplicationContext application)
            : base(root)
        {
            var parts = new string[] { application.PhysicalPath }.Concat(root.Parts).ToArray();
            FileLocation = Path.Combine(parts);
        }

        /// <summary>
        ///  Creates a new <see cref="FileResourceLocation"/> serving up files from a
        ///  physical path at an arbitrary location on the filesystem.
        /// </summary>
        /// <param name="root">
        ///  The root URL for the files. This must be app-relative.
        /// </param>
        /// <param name="fileLocation">
        ///  The location in the filespace where the files are located.
        /// </param>

        public FileResourceLocation(VirtualPath root, string fileLocation)
            : base(root)
        {
            FileLocation = fileLocation;
        }

        /// <summary>
        ///  Creates a new <see cref="FileResourceLocation"/> serving up files from a
        ///  virtual path within the application root directory, but at a different URL.
        /// </summary>
        /// <param name="root">
        ///  The root URL for the files. This must be app-relative.
        /// </param>
        /// <param name="fileLocation">
        ///  The location in the application where the files are located. This may be either
        ///  app-relative or request-relative; in the latter case, it will be taken as being
        ///  relative to the root.
        /// </param>
        /// <param name="application"></param>

        public FileResourceLocation(VirtualPath root, VirtualPath fileLocation, IApplicationContext application)
            : base(root)
        {
            switch (fileLocation.Type) {
                case VirtualPathType.Absolute:
                    throw new ArgumentException("The file location must be within the application.", "fileLocation");
                case VirtualPathType.RequestRelative:
                    fileLocation = root.Append(fileLocation);
                    break;
            }

            var parts = new string[] { application.PhysicalPath }.Concat(fileLocation.Parts).ToArray();
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
