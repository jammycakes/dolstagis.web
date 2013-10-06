using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dolstagis.Web
{
    public class ViewResult
    {
        public VirtualPath Path { get; private set; }

        public object Model { get; private set; }

        public IDictionary<string, object> Data { get; private set; }

        public ViewResult(string path)
        {
            this.Path = new VirtualPath(path);
            this.Model = null;
            this.Data = new Dictionary<string, object>();
        }

        public ViewResult(string path, object model)
        {
            this.Path = new VirtualPath(path);
            this.Model = model;
            this.Data = new Dictionary<string, object>();
        }
    }
}
