using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web.Views.Static
{
    public class FilespaceResourceLocation : IResourceLocation
    {
        private string _root;

        public FilespaceResourceLocation(string root)
        {
            _root = root;
        }

        public IResource Get(string path, string appRoot)
        {
            string absPath = Path.Combine(
                appRoot,
                _root.Replace("/", Path.DirectorySeparatorChar.ToString()),
                path.Replace("/", Path.DirectorySeparatorChar.ToString())
            );

            if (File.Exists(absPath)) {
                return new FileResource(absPath);
            }
            else {
                return null;
            }
        }
    }
}
