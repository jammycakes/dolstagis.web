using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dolstagis.Web.Static;

namespace Dolstagis.Web.Views.Static
{
    public class FilespaceResourceLocation : IResourceLocation
    {
        private string _root;
        private bool _isAbsolute;

        public FilespaceResourceLocation(string root)
        {
            _root = root;
            _isAbsolute = Path.IsPathRooted(root);
        }

        public IResource Get(string path, string appRoot)
        {
            string absPath;
            if (_isAbsolute) {
                absPath = Path.Combine(
                    appRoot,
                    _root.Replace("/", Path.DirectorySeparatorChar.ToString()),
                    path.Replace("/", Path.DirectorySeparatorChar.ToString())
                );
            }
            else {
                absPath = Path.Combine(
                    appRoot,
                    _root.Replace("/", Path.DirectorySeparatorChar.ToString()),
                    path.Replace("/", Path.DirectorySeparatorChar.ToString())
                );
            }

            if (File.Exists(absPath)) {
                return new FileResource(absPath);
            }
            else {
                return null;
            }
        }
    }
}
